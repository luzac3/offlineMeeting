import { SignalR } from "@root/share/SignalR";
import { GenerateQr } from "./GenerateQr";
import { GetRegister } from "./GetRegister";

(() => {
    const signalR = new SignalR();
    const generateQr = new GenerateQr();
    const getRegister = new GetRegister();
    signalR.activate();
    generateQr.generate();
    getRegister.registerComplete(signalR);
})();