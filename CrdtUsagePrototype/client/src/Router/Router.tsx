import { createBrowserRouter } from 'react-router-dom';
import ClientFormFeature from '../3-Features/ClientForm/ClientFormFeature';
import App from '../0-App/App';
import DocumentFeature from '../3-Features/Document/DocumentFeature';
import DocumentWithVersionsFeature from '../3-Features/DocumentWithVersions/DocumentWithVersions';
import ClientFormPage from '../2-Pages/ClientFormPage';
import DocumentPage from '../2-Pages/DocumentPage';


export const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      { index: true, element: <ClientFormPage /> },
      { path: 'document/', element: <DocumentPage /> },
    ]
  }
])

export const ROUTES = {
  MAIN: "/",
  DOCUMENT: "document/"
};