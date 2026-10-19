import { contextBridge, ipcRenderer } from 'electron';

contextBridge.exposeInMainWorld('electronAPI', {
    printReceipt: (args: { resultIds: string[] }) => ipcRenderer.invoke('printReceipt', args),
    printCheckinQr: (args: { userCd: string }) => ipcRenderer.invoke('printCheckinQr', args),
    getMenuItems: () => ipcRenderer.invoke('getMenuItems'),
    openPage: (url: string) => ipcRenderer.invoke('openPage', url)
});