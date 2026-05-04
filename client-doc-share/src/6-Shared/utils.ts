import { AccountApi, Configuration } from "./ApiClient";

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

export function generateAccountApi() : AccountApi {
  const conf = new Configuration({
    basePath: import.meta.env.VITE_API_URL
  });
  return new AccountApi(conf);
} 