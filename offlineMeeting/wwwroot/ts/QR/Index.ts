import { GenerateQrList } from "./GenerateQrList";
import { SignalR } from "@root/share/SignalR";
import { Checkin } from "./Checkin";
import { CheckinNotify } from "./CheckinNotify";

(() => {
    const generateQrList = new GenerateQrList();
    generateQrList.generate();

    const signalR = new SignalR();
    const checkin = new Checkin();
    const checkinNotify = new CheckinNotify();

    signalR.activate().then(() => {
        checkin.setButtons();
        checkinNotify.listen(signalR);
    });
})();