import type { ExceptionDto } from "./ApiClient"

let staticTranslates : Map<string, string> = new Map<string, string>()
// here contains static translates
// staticTranslates.set("example", "пример")
staticTranslates.set("Request contains client error.", 
  "Запрос был составлен неверно")
staticTranslates.set("The field UserName must be a string or collection type with a minimum length of '3' and maximum length of '25'.",
  "Имя пользователя должно иметь длину от 3 до 25 символов")
staticTranslates.set("The Email field is not a valid e-mail address.",
  "Введен неправильный адрес электронной почты")
staticTranslates.set("You do not have permission to access this resource.", 
  "У вас недостаточно прав для доступа к этому ресурсу")
staticTranslates.set("The requested object not found.", 
  "Запрошенный объект не был найден")
staticTranslates.set("Server side error occured.", 
  "Произошла ошибка на стороне сервера")
staticTranslates.set("Check your registration details for correctness.", 
  "Перепроверьте учетные данные. Логин или пароль были указаны неверно")

staticTranslates.set("Couldn't create user from provided creditensials", 
  "Не получилось создать пользователя. Предоставленные данные не верны")
staticTranslates.set("Reference was not found.", 
  "Ссылка не была найдена")
staticTranslates.set("This owner already created such reference", 
  "У Вас уже имеется ссылка с данным url")
staticTranslates.set("This owner already has reference with such nickname", 
  "Данный псевдоним уже используется для другой ссылки")
staticTranslates.set("The Password field is required.", 
  "Заполните пароль")
staticTranslates.set("The UserName field is required.", 
  "Заполните имя пользователя")
staticTranslates.set("The field UserName must be a string or collection type with a minimum length of '5' and maximum length of '15'.", 
  "Имя пользователя должно быть строкой длиной от 5 до 15 символов")
staticTranslates.set("Passwords must be at least 8 characters.", 
  "Пароль должен иметь как минимум 8 символов в длину")
staticTranslates.set("Passwords must have at least one non alphanumeric character.", 
  "Пароль должен иметь как минимум один специальный символ ('@', '#'...)")
staticTranslates.set("Passwords must have at least one lowercase ('a'-'z').", 
  "Пароль должен иметь как минимум один незаглавный символ")
staticTranslates.set("Passwords must have at least one uppercase ('A'-'Z').", 
  "Пароль должен иметь как минимум один заглавный символ")
staticTranslates.set("Passwords must have at least one digit ('0'-'9').", 
  "Пароль должен иметь как минимум одну цифру")
staticTranslates.set("The Nickname field is required.", 
  "Заполните, пожалуйста, поле псевдонима")
staticTranslates.set("Nickname length should be between 5 and 40.", 
  "Псевдоним должен быть длиной от 5 до 40 символов")
staticTranslates.set("The Reference field is required.", 
  "Заполните ссылку для сокращения")
staticTranslates.set("Provided reference does not represent valid reference.", 
  "Перепроверьте, пожалуйста, формат вашей ссылки")
staticTranslates.set("Provided nickname does not represent valid nickname.", 
  "Псевдоним должен иметь следующий формат: 'ozon-book' или например 'книга-из-озона'. Перепроверьте, пожалуйста, псведоним на соответствие")
staticTranslates.set("The authorization time has expired. New authorization needed.", 
  "Было просрочено время авторизации. Требуется повторная авторизация")

staticTranslates.set("Service is unavaliable for the current moment", 
  "Сервис на текущий момент недоступен. Повторите попытку позже")

let templateTranslates : {[Key: string] : string} = {}
// here contains template translates
// templateTranslates['Some example object has width {0}, but should have length equal to 2'] = 
//   "Некоторый объект для примера имеет длину {0}, но должна быть длина, равная двум"
templateTranslates['Username {0} is already taken.'] = 
  "Имя пользователя {0} уже используется"
templateTranslates['Email {0} is already taken.'] = 
  "Электронная почта {0} уже используется"

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