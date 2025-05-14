import * as monaco from 'monaco-editor';


export const getSyntaxes = () : string[] => {
    const syntaxes : string[] = monaco.languages.getLanguages().map(l => l.id);
    return syntaxes.sort(((lhs, rhs) => lhs.localeCompare(rhs)))
};
