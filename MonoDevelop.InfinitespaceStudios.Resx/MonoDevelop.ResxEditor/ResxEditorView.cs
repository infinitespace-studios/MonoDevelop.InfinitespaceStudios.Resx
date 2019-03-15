using System.Resources;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui;
using System.Threading.Tasks;


namespace MonoDevelop.ResxEditor
{
	public class ResxEditorView : ViewContent
	{
		readonly ResxEditorWidget widget;

		public override Control Control 
		{ 
			get 
			{ 
				return widget; 
			} 
		}

		public ResxEditorView()
		{
			widget = new ResxEditorWidget(this);
		}

		public override Task Load (FileOpenInformation fileOpenInformation)
		{
			widget.SetResxInfo (fileOpenInformation.FileName);
			ContentName = fileOpenInformation.FileName;
			IsDirty = false;
			return base.Load (fileOpenInformation);
		}

		public override Task Save (FileSaveInformation fileSaveInformation)
		{
			var fileName = fileSaveInformation.FileName;
			ResXDataNode [] nodes = widget.GetResxInfo (fileName);

			using (ResXResourceWriter resxWriter = new ResXResourceWriter (fileName)) {
				foreach (ResXDataNode node in nodes) {
					resxWriter.AddResource (node);
				}

				resxWriter.Generate ();
			}

			ContentName = fileName;
			IsDirty = false;
			return base.Save (fileSaveInformation);
		}
	}
}