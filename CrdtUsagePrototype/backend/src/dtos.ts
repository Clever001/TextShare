export interface MessageDto {
  type: MessageType
}

export enum MessageType {
  NewMessage, SomeOtherMessage, YjsMessage, 
}

export interface NewMessage extends MessageDto {
  type: MessageType.NewMessage,
  content: string
}