import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { GetNewOrder } from "./GetNewOrder";
import { AudioStart } from "./AudioStart";
import { OrderComplete } from "./OrderComplete";


(async () => {
    const ctx = new (window.AudioContext || (window as any).webkitAudioContext)();
    new ControlNavbar();
    new OrderComplete();
    const signalR = new SignalR();
    const getNewOrder = new GetNewOrder();
    const audioStart = new AudioStart();
    signalR.activate();
    getNewOrder.GetNewOrder(signalR, ctx);
    audioStart.Play(ctx);
})();