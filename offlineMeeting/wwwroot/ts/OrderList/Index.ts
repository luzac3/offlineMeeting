import { ControlNavbar } from "@root/share/ControlNavbar";
import { BackButton } from "@root//share/BackButton";
import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";


(async () => {
    new ControlNavbar();
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();
    const backButton = new BackButton();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
    backButton.Buck();
})();