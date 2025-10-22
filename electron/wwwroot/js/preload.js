const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electronAPI', {
    // args = { template: 'order.html', apiPath: '/Pos/PrintData?orderId=123', headers: { Authorization: 'Bearer ...' } }
    printWithTemplate: (args) => ipcRenderer.invoke('print-with-template', args)
});