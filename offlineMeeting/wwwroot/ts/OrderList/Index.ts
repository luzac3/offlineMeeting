import { ControlNavbar } from "@root/share/ControlNavbar";
import { BackButton } from "@root//share/BackButton";
import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";


(async () => {
    new ControlNavbar();
    const backButton = new BackButton();
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();
    backButton.Buck();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();