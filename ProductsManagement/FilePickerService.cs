using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace ProductsManagement;

public class FilePickerService
{
    public async Task<IStorageFile?> OpenFileAsync(Window parent)
    {
        var storageProvider = parent.StorageProvider;
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите файл",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("XML файлы") {Patterns = ["*.xml", "*.XML"] }
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
            Title = "Сохранить XML",
            DefaultExtension = "*.xml",
            SuggestedFileName = "your_products.xml",
            FileTypeChoices = 
            [
                new FilePickerFileType("XML файлы") {Patterns = ["*.xml", "*.XML"] }
            ]
        });

        if (file == null) return;
        await table.SaveXmlFile(file);
    }
}