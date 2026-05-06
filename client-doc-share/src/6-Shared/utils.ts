import axios, { AxiosError } from "axios";
import { AccountApi, Configuration, DocumentApi, type ExceptionDto } from "./ApiClient";
import { translateException } from "./errorTranslator";

function toStringWithPad(num: number): string {
  return num.toString().padStart(2, "0");
}

export function dateToString(d: Date): string {
  const days: string = toStringWithPad(d.getDay());
  const months: string = toStringWithPad(d.getMonth() + 1);

  return `${days}.${months}.${d.getFullYear()}`;
}

export function dateToStringWithTime(d: Date): string {
  const hours: string = toStringWithPad(d.getHours());
  const minutes: string = toStringWithPad(d.getMinutes());
  const seconds: string = toStringWithPad(d.getSeconds());

  return `${dateToString(d)} ${hours}:${minutes}:${seconds}`;
}

function getDefaultApiConf() : Configuration {
  return new Configuration({
    basePath: import.meta.env.VITE_API_URL
  })
}

function getAuthApiConf(token: string) : Configuration {
  return new Configuration({
    basePath: import.meta.env.VITE_API_URL,
    accessToken: token
  })
}

export function generateAccountApi() : AccountApi {
  return new AccountApi(getDefaultApiConf())
}

export function generateDocumentApiAuth(token: string) : DocumentApi {
  return new DocumentApi(getAuthApiConf(token))
}

export function generateDocumentApi() : DocumentApi {
  return new DocumentApi(getDefaultApiConf())
}

export function isUnauthError(err: any) : boolean {
  if (!axios.isAxiosError(err)) {
    return false
  }
  const error = err as AxiosError
  if (!error.response) {
    return false
  } else {
    return err.response!.status === 401
  }
} 

export function getApiErrors(err: any) : string[] {
  if (!axios.isAxiosError(err)) {
    return ["Произошла неизвестная ошибка"]
  }
  const error = err as AxiosError
  if (!error.response) {
    return ["Не удалось получить ответ от сервера"]
  }
  if (error.response.status == 500) {
    return ["Ошибка на сервере"]
  } else {
    const exception = translateException(error.response.data as ExceptionDto)

    if (exception.details && exception.details.length > 0) {
      return exception.details
    } else {
      return [exception.description]
    }
  }
}