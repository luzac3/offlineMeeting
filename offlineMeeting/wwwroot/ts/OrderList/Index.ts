import { SignalR } from "@root/share/SignalR";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";


(async () => {
    new ControlNavbar();
    const signalR = new SignalR();
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();
    signalR.activate();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();