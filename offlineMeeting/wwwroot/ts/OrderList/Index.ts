import { GetOrderList } from "./GetOrderList";
import { SendOrder } from "./SendOrder";


(async () => {
    const getOrderList = new GetOrderList();
    const sendOrder = new SendOrder();

    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();