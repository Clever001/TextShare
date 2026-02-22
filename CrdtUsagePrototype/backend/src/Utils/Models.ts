import { VersionDto } from "./Dtos"
import { YjsDocumentKeeper } from "./YjsDocumentKeeper"

export type Version = {
  id: string,
  name: string,
  createdTime: number,
}

export type VersionToDocument = {
  version: Version,
  documentState: Uint8Array<ArrayBufferLike>,
}

