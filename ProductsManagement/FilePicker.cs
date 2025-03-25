using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Model;

namespace ProductsManagement;

public class FilePicker
{
    public async Task<IStorageFile?> OpenFileAsync(Window parent)
    {
        var storageProvider = parent.StorageProvider;
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open file",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("XML files") {Patterns = ["*.xml", "*.XML"] }
            ]
        });

        if (files.Count == 0) return null;
        return files[0];
    }

    public async Task SaveFileAsync(Window parent, ProductsTable table)
    {
        var storageProvider = parent.StorageProvider;
        var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save XML",
            DefaultExtension = "*.xml",
            SuggestedFileName = "your_products.xml",
            FileTypeChoices = 
            [
                new FilePickerFileType("XML files") {Patterns = ["*.xml", "*.XML"] }
            ]
        });

        if (file == null) return;
        await table.SaveXmlFile(file);
    }
}