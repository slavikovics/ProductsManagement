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
            Title = "Select a file",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("XML files") {Patterns = [".xml", ".XML"] }
            ]
        });

        if (files.Count == 0) return null;
        return files[0];
    }
}