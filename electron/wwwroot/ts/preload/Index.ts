import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
    printReceipt: (url: string) => ipcRenderer.invoke('printReceipt', url)
});