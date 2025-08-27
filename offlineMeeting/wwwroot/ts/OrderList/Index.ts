import { ControlNavbar } from "@root/share/ControlNavbar";
import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";


(async () => {
    new ControlNavbar();
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();