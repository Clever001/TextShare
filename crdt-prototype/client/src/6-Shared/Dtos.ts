export type UserDto = {
  name: string,
  cursorColor: string,
};

export interface MessageDto {
  type: MessageType
}

export type MessageType =
  | 'createNewVersion'
  | 'newVersionCreated'
  | 'versionsList'
  | 'sendCertainVersion'
  | 'sendedCertainVersion'
  | 'deleteCertainVersion'
  | 'deletedCertainVersion'
  | 'renameCertainVersion'
  | 'renamedCertainVersion'
  | 'rollbackToVersion'
  | 'rollbackedToVersion';

export interface VersionDto extends MessageDto {
  type: "createNewVersion" | "newVersionCreated",
  id: string,
  name: string,
  createdTime: number,
}

export interface VersionsListDto extends MessageDto {
  type: "versionsList",
  versions: Version[],
}

export interface SendVersionRequest extends MessageDto {
  type: "sendCertainVersion",
  versionId: string,
}

export interface VersionState extends MessageDto {
  type: "sendedCertainVersion",
  versionState: string,
}

export interface DeleteVersionRequest extends MessageDto {
  type: 'deleteCertainVersion' | 'deletedCertainVersion',
  versionId: string,
}

export interface RenameVersionRequest extends MessageDto {
  type: 'renameCertainVersion',
  versionId: string,
  newVersionName: string,
}

export interface RenamedVersionRequest extends MessageDto {
  type: 'renamedCertainVersion'
  id: string,
  name: string,
  createdTime: number,
}

export interface RollbackDocRequest extends MessageDto {
  type: 'rollbackToVersion',
  versionIdToRollback: string,
}

export interface RollbackedDocRequest extends MessageDto {
  type: 'rollbackedToVersion',
  id: string,
  name: string,
  createdTime: number,
}

export type Version = {
  id: string,
  name: string,
  createdTime: number,
}
