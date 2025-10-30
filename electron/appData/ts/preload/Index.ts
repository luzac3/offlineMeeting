import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
    printReceipt: (args: { resultIds: string[] }) => ipcRenderer.invoke('printReceipt', args)
});