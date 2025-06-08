import { ExceptionDto } from "../Dtos";

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
staticTranslates.set("Title cannot be empty.",
    "Заголовок не может быть пустым.")
staticTranslates.set("Text with Composite of fields Title and AppUserId already exists.",
    "Текст с таким названием уже существует.")
staticTranslates.set("You do not have permission to access this resource.",
    "У вас недостаточно прав для выполняния данного действия.")
staticTranslates.set("Passwords must have at least one digit ('0'-'9').",
    "Пароль должен содержать хотя бы одну цифру ('0'-'9').");
staticTranslates.set("The expiry date cannot be later than the current date + 10 minutes.",
    "Дата и время удаления текста не могут быть выставлены ранее, чем текущее время + 10 минут.");
staticTranslates.set("The Title field is required.",
    "Поле названия обязательно для заполнения.");
staticTranslates.set("Cannot create user with provided information.",
    "Не могу создать пользователя с данной информацией");
staticTranslates.set("Account with such userName already exists.",
    "Аккаунт с таким именем пользователя уже существует");
staticTranslates.set("Account with such email already exists.",
    "Аккаунт с токой электронной почтой уже существует");
staticTranslates.set("Password cannot be empty.",
    "Пароль не может быть пустым");
staticTranslates.set("Current user not found.",
    "Текущий пользователь не был найден");
staticTranslates.set("Text already exists.",
    "Текст с заданными метаданными уже существуют");
staticTranslates.set("Text with Composite of fields Title and AppUserId already exists.",
    "Текст с таким названием уже существует у данного автора");
staticTranslates.set("Text not found.",
    "Текст не был найден");
staticTranslates.set("Sender not found.",
    "Отправитель запроса не был найден");
staticTranslates.set("Invalid Sort By field.",
    "Неверно введенное поле для сортировки");
staticTranslates.set("This Title already exists.",
    "Текст с таким названием уже существует");
staticTranslates.set("Provided password is not correct.",
    "Предоставленные пароль неверен");
staticTranslates.set("First and second users cannot be same.",
    "Вы не можете отправить запрос самому себе");
staticTranslates.set("First user not found.",
    "Первый пользователь, добавляемый в друзья, не найден");
staticTranslates.set("Second user not found.",
    "Второй пользователь, добавляемый в друзья, не найден");
staticTranslates.set("User is already in friends list.",
    "Пользователи уже являются друзьями");
staticTranslates.set("User is not in friends list.",
    "Пользователи уже не являются друзьями");
staticTranslates.set("Sender name and recipient name cannot be the same.",
    "Отправитель и получатель не могут быть одним лицом");
staticTranslates.set("Recipient not found.",
    "Получатель не найден");
staticTranslates.set("Users are friends already.",
    "Пользователи уже являются друзьями");
staticTranslates.set("Request already exists.",
    "Запрос в друзья уже существует");
staticTranslates.set("Reverse request already exists.",
    "Reverse request already exists.");
staticTranslates.set("Did not exist from the beginning.",
    "Не существовал изначально");
staticTranslates.set("Request not found.", 
    "Запрос не был найден");
staticTranslates.set("Invalid expiry date format.",
    "Неверная дата удаления текста");
staticTranslates.set("Tags list cannot contain more then 5 elements.",
    "Список тегов не может быть длинее 5");
staticTranslates.set("Each tag length should be between 1 and 20",
    "Каждый тег должен быть длиной от 1 до 20 символов");
staticTranslates.set("The field Description must be a string or array type with a maximum length of '250'.",
    "Описания не может быть длиннее 250 символов");
staticTranslates.set("The field Title must be a string or array type with a maximum length of '70'.",
    "Название не может быть длиннее 70 символов");

let templateTranslates : {[Key: string] : string} = {};
templateTranslates["Username '{0}' is already taken."] = "Имя '{0}' уже используется другим пользователем";
templateTranslates["Email '{0}' is already taken."] = "Почта '{0}' уже используется другим пользователем";
templateTranslates["{0} cannot be empty."] = "'{0}' не может быть пустым";

export const translateException = (e : ExceptionDto) : ExceptionDto => {
    e.description = translateString(e.description);
    if (e.details) {
        e.details = e.details.map(s => translateString(s))
    }

    return e;
}

const translateString = (s: string) : string => {
    if (staticTranslates.has(s)) {
        return staticTranslates.get(s) ?? "Неизвестная ошибка во время перевода строки. " + 
            "Перевод был найден, но его не удалось получить.";
    }

    for (const [template, translation] of Object.entries(templateTranslates)) {
        const regex = new RegExp(`^${template.replace(/\{(\d+)\}/g, "(.+)")}$`);
        const match = s.match(regex);
        if (match) {
            return translation.replace(/\{(\d+)\}/g, (_, index) => match[parseInt(index) + 1]);
        }
    }

    return s
}