namespace backend.Services.Interfaces.Util;

public interface IDesafiosBackupService
{
    Task<string?> BackupDesafiosGenerateAsync();
    Task<int> RestoreAsync(string backupDesafios);
    string? BackupQuizzesGenerateAsync();
    int RestoreQuizzesAsync(string backupQuizzes);
}
