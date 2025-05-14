let staticTranslates : Map<string, string> = new Map<string, string>();

staticTranslates.set("The Password field is required.", "Пароль не может быть пустым");
staticTranslates.set("The UserNameOrEmail field is required.", "Введите логин или электронную почту");
staticTranslates.set("Check your registration details for correctness.", "Логин или пароль неверны");

staticTranslates.set("The Email field is required.", "Введите электронную почту");
staticTranslates.set("The Email field is not a valid e-mail address.", "Введенное значение в поле почты не является электронной почтой");
staticTranslates.set("The UserName field is required.", "Введите имя польователя");
staticTranslates.set("The field UserName must be a string or collection type with a minimum length of '5' and maximum length of '25'.", 
    "Имя пользователя должно быть длиной минимум в 5 символов, максимум в 25 символов");
staticTranslates.set("Passwords must be at least 8 characters.", "Пароль должен содержать минимум 8 символов.");
staticTranslates.set("Passwords must have at least one non alphanumeric character.", 
    "Пароли должны содержать по крайней мере один не буквенно-цифровой символ.")
staticTranslates.set("Passwords must have at least one lowercase ('a'-'z').",
    "В паролях должна быть хотя бы одна строчная буква ('a'- 'z').");
staticTranslates.set("Passwords must have at least one uppercase ('A'-'Z').",
    "В паролях должна быть хотя бы одна заглавная буква ('A'-'Z').");
staticTranslates.set("Password is not provided.", 
    "Пароль не был предоставлен.")

let templateTranslates : {[Key: string] : string} = {};
templateTranslates["Username '{0}' is already taken."] = "Имя '{0}' уже используется другим пользователем";
templateTranslates["Email '{0}' is already taken."] = "Почта '{0}' уже используется другим пользователем";

export const translateException = (e : string) : string => {
    if (staticTranslates.has(e)) {
        return staticTranslates.get(e) ?? "Unexpected error occured.";
    }

    for (const [template, translation] of Object.entries(templateTranslates)) {
        const regex = new RegExp(`^${template.replace(/\{(\d+)\}/g, "(.+)")}$`);
        const match = e.match(regex);
        if (match) {
            return translation.replace(/\{(\d+)\}/g, (_, index) => match[parseInt(index) + 1]);
        }
    }

    return e;
}

export const translateExceptions = (e: string[]) : string[] => {
    return e.map(translateException);
}