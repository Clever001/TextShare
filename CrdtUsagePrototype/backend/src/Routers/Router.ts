import { ClientSocket } from "../BaseServer";

export interface Router {
  route(rawData: Buffer, currentClient: ClientSocket): void;
}