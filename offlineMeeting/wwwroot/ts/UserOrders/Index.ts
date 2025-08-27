import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { GetNewOrder } from "./GetNewOrder";


(async () => {
    new ControlNavbar();
    const signalR = new SignalR();
    new GetNewOrder();
    signalR.activate();
})();