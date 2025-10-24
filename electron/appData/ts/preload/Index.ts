import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
    printReceipt: (args: { resultId: string[] }) => ipcRenderer.invoke('printReceipt', args)
});