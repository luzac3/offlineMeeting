import { SignalR } from "@root/share/SignalR";
import { ControlModal } from "@root/share/ControlModal";
import { ControlNavbar } from "@root/share/ControlNavbar";
import { CaliculateCoinNumber } from "./CaliculateCoinNumber";
import { StockOrder } from "./StockOrder";


(async () => {
    new ControlNavbar();
    const signalR = new SignalR();
    const controlModal = new ControlModal();
    const caliculateCoinNumber = new CaliculateCoinNumber();
    const stockOrder = new StockOrder();

    controlModal.setControl();
    caliculateCoinNumber.setCoin();
    stockOrder.addOrder();
})();