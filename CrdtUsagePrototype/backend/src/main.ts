import { BaseServerBuilder } from './BaseServer/BaseServerBuilder';
import { VersionsKeeper } from './Utils/VersionsKeeper';
import { YjsDocumentKeeper } from './Utils/YjsDocumentKeeper';
import dotenv from 'dotenv';

dotenv.config({
  path: ['.env.local', '.env']
});

const initialDocument = YjsDocumentKeeper.createEmptyDocument();
const versionsKeeper = new VersionsKeeper();

const yjsHost = process.env.YJS_HOST;
const yjsPort = process.env.YJS_PORT;
const messagesHost = process.env.MESSAGES_HOST;
const messagesPort = process.env.MESSAGES_PORT;
if (!yjsHost || !yjsPort || !messagesHost || ! messagesPort) {
  throw new Error("Check '.env' file. Mayby some constants are not initialized.");
}

const yjsServer = new BaseServerBuilder()
  .withHostAndPort(yjsHost, Number.parseInt(yjsPort))
  .withDocument(initialDocument)
  .withYjsRouter()
  .withLoggerName('yjsServer')
  .build();

const massagesServer = new BaseServerBuilder()
  .withHostAndPort(messagesHost, Number.parseInt(messagesPort))
  .withDocument(initialDocument)
  .withVersionsKeeper(versionsKeeper)
  .withVersionsRouter()
  .withLoggerName('massagesServer')
  .build();
