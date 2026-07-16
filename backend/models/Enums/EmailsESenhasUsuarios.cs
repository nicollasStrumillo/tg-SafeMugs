using System.ComponentModel.DataAnnotations;

namespace backend.models.Enums;

public enum EmailsESenhasUsuarios
{
    [Display(Name = "ana.lopes@safemugs.com", Description = "5858ea228cc2edf88721699b2c8638e5")] // --> welcome123 -> Senha comum pra utilizar no Desafio de brute force
    AnaLopes,

    [Display(Name = "bruno.costa@safemugs.com", Description = "482c811da5d5b4bc6d497ffa98491e38")] // --> password123 -> Senha comum pra utilizar no Desafio de brute force
    BrunoCosta,

    [Display(Name = "carla.mendes@safemugs.com", Description = "37b4e2d82900d5e94b8da524fbeb33c0")]  // --> football -> Senha comum pra utilizar no Desafio de brute force
    CarlaMendes,

    [Display(Name = "diego.souza@safemugs.com", Description = "cc25c0f861a83f5efadc6e1ba9d1269e")] // --> monkey123 -> Senha comum pra utilizar no Desafio de brute force
    DiegoSouza,

    [Display(Name = "elisa.martins@safemugs.com", Description = "3fc0a7acf087f549ac2b266baf94b8b1")] // --> qwerty123 -> Senha comum pra utilizar no Desafio de brute force
    ElisaMartins,

    [Display(Name = "felipe.rocha@safemugs.com", Description = "0571749e2ac330a7455809c6b0e7af90")]  // --> sunshine -> Senha comum pra utilizar no Desafio de brute force
    FelipeRocha,

    [Display(Name = "marina.alves@safemugs.com", Description = "8afa847f50a716e64932d995c8e7435a")] // --> princess -> Senha comum pra utilizar no Desafio de brute force
    MarinaAlves,

    [Display(Name = "admin@email.com", Description = "df49d9fce01a137041d6d89e6629abbf")] // --> SafeAdminMugs!
    Admin

}
