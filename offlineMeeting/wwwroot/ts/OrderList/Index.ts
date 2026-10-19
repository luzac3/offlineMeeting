import { BackButton } from "@root//share/BackButton";
import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";

(async () => {
    const backButton = new BackButton();
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();
    backButton.Buck();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();