import pino from "pino";
import { ClientSocket } from "../BaseServer";
import { MessageDto, MessageType } from "../dtos"
import { Router } from "./Router";

export type MessageHandler = (dto: MessageDto, currentClient: ClientSocket) => void;

export class MessagesRouter implements Router {
  private readonly logger = pino({name: MessagesRouter.name});
  private readonly handlers = new Map<MessageType, MessageHandler>();

  public on(
    messageType: MessageType,
    handler: MessageHandler
  ): void {
    this.handlers.set(messageType, handler);
  }

  /**
   * Routes websocket message.
   * @param rawData websocket raw message data
   * @returns True if router handeled message successfully. 
   * Returns false if router cannot handle message.
   */
  public route(rawData: Buffer, currentClient: ClientSocket): void {
    let data: MessageDto;

    try {
      const parsedData = JSON.parse(rawData.toString("utf-8"));
      if ('type' in parsedData) {
        data = parsedData as MessageDto;
      } else {
        this.throwInvalidMessage();
        return;
      }
    } catch (e) {
      if (e instanceof SyntaxError) {
        this.throwInvalidMessage();
        return;
      } else {
        throw e;
      }
    }

    this.logger.info("Got data type: " + data.type);
    const handler = this.handlers.get(data.type);
    
    if (!handler) {
      const errorMessage = `Unknown message type: ${data.type}`;
      this.logger.error(errorMessage);
      throw new Error(errorMessage);
    }

    handler(data, currentClient);
  }

  private throwInvalidMessage(): void {
    throw new Error(`Invalid message content.`);
  }
}