import { BaseServerBuilder } from './BaseServer/BaseServerBuilder';
import { VersionsKeeper } from './Utils/VersionsKeeper';
import { YjsDocumentKeeper } from './Utils/YjsDocumentKeeper';

const initialDocument = YjsDocumentKeeper.createEmptyDocument();
const versionsKeeper = new VersionsKeeper();

const yjsServer = new BaseServerBuilder()
  .withHostAndPort('192.168.0.227', 1234)
  .withDocument(initialDocument)
  .withYjsRouter()
  .withLoggerName('yjsServer')
  .build();

const massagesServer = new BaseServerBuilder()
  .withHostAndPort('192.168.0.227', 1235)
  .withDocument(initialDocument)
  .withVersionsKeeper(versionsKeeper)
  .withVersionsRouter()
  .withLoggerName('massagesServer')
  .build();
