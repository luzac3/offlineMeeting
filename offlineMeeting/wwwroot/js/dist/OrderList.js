/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./wwwroot/ts/OrderList/GetOrderList.ts":
/*!**********************************************!*\
  !*** ./wwwroot/ts/OrderList/GetOrderList.ts ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   GetOrderList: () => (/* binding */ GetOrderList)
/* harmony export */ });
/* harmony import */ var _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/ControlNavbar */ "./wwwroot/ts/share/ControlNavbar.ts");
/* harmony import */ var _root_share_Counter__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @root/share/Counter */ "./wwwroot/ts/share/Counter.ts");
/* harmony import */ var _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @root/share/LocalStrage */ "./wwwroot/ts/share/LocalStrage.ts");
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");




class GetOrderList {
    constructor() {
        this.setOrderList = () => {
            const counter = new _root_share_Counter__WEBPACK_IMPORTED_MODULE_1__.Counter();
            let orderEntityList = [];
            if (_root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_2__.LocalStrage.check("OrderEntityList")) {
                orderEntityList = _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_2__.LocalStrage.get("OrderEntityList");
            }
            let totalCoins = 0;
            this.send(orderEntityList).then((data) => {
                document.getElementById("order_list").innerHTML = data;
                document.getElementById("pon_coins").innerText = totalCoins.toString();
                document.getElementById("pon_coins_input").value = totalCoins.toString();
                // カウンターをアクティブ
                counter.SetCounter();
                // ナビバーの位置を調整
                new _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__.ControlNavbar();
            });
            orderEntityList.forEach(order => {
                totalCoins += order.OrderCoins;
            });
        };
        this.send = async (orderEntityList) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_3__.FetchApi();
            return await fetchApi.send(this.url, this.method, this.headers, orderEntityList, this.responseKind).then(async (data) => {
                return data;
            }).catch(e => {
                throw e;
            });
        };
        this.url = '/Pos/OrderListContents';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/OrderList/SendOrder.ts":
/*!*******************************************!*\
  !*** ./wwwroot/ts/OrderList/SendOrder.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   SendOrder: () => (/* binding */ SendOrder)
/* harmony export */ });
/* harmony import */ var _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/LocalStrage */ "./wwwroot/ts/share/LocalStrage.ts");
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");
/* harmony import */ var _pos_OrderEntity__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../pos/OrderEntity */ "./wwwroot/ts/pos/OrderEntity.ts");



class SendOrder {
    constructor() {
        this.setOrderList = () => {
            document.getElementById("send_order")?.addEventListener("click", async () => {
                const orderElements = Array.from(document.getElementsByClassName("order"));
                let orderEntityList = [];
                if (orderElements.length > 0) {
                    Array.from(document.getElementsByClassName("order")).forEach((orderElement) => {
                        const alcoholAmountElement = orderElement.querySelector(".alcohol_amount");
                        const alcoholAmount = alcoholAmountElement ? parseInt(alcoholAmountElement.value) : 0;
                        orderEntityList.push(new _pos_OrderEntity__WEBPACK_IMPORTED_MODULE_2__.OrderEntity(parseInt(orderElement.dataset.result_id), parseInt(orderElement.querySelector('.category')?.value ?? "0"), parseInt(orderElement.querySelector(".order_number").value), parseInt(orderElement.querySelector(".order_coins").value), alcoholAmount));
                    });
                    const orderObjectList = orderEntityList.map(order => ({
                        ResultId: order.ResultId,
                        Category: order.Category,
                        OrderNumber: order.OrderNumber,
                        OrderCoins: order.OrderCoins,
                        AlcoholAmount: order.AlcoholAmount
                    })).filter(x => x.OrderNumber != 0);
                    this.send(orderObjectList).then((data) => {
                        const result = JSON.parse(data);
                        if (result.status == "200") {
                            // オーダーのストックをクリア
                            _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__.LocalStrage.delete();
                            // 画面をクリア
                            document.getElementById("order_list").innerHTML = "";
                            document.getElementById("pon_coins").innerText = "0";
                            document.getElementById("pon_coins_input").value = "0";
                        }
                        window.alert(result.message);
                    });
                }
            });
        };
        this.send = async (orderEntityList) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_1__.FetchApi();
            return await fetchApi.send(this.url, this.method, this.headers, orderEntityList, this.responseKind).then(async (data) => {
                return data;
            }).catch(e => {
                throw e;
            });
        };
        this.url = '/Pos/SendOrder';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/pos/OrderEntity.ts":
/*!***************************************!*\
  !*** ./wwwroot/ts/pos/OrderEntity.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   OrderEntity: () => (/* binding */ OrderEntity)
/* harmony export */ });
class OrderEntity {
    constructor(resultId, category, orderNumber, orderCoins, alcoholAmount = 0) {
        this.ResultId = resultId;
        this.Category = category;
        this.OrderNumber = orderNumber;
        this.OrderCoins = orderCoins;
        this.AlcoholAmount = alcoholAmount;
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/BackButton.ts":
/*!****************************************!*\
  !*** ./wwwroot/ts/share/BackButton.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   BackButton: () => (/* binding */ BackButton)
/* harmony export */ });
class BackButton {
    constructor() {
        this.Buck = () => {
            const element = document.getElementById('back');
            element.addEventListener("click", () => {
                history.back();
            });
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/ControlNavbar.ts":
/*!*******************************************!*\
  !*** ./wwwroot/ts/share/ControlNavbar.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ControlNavbar: () => (/* binding */ ControlNavbar)
/* harmony export */ });
class ControlNavbar {
    constructor() {
        this.setPadding = () => {
            const navbar = document.querySelector('.navbar');
            let subbarHeight = 0;
            if (navbar) {
                const subbar = document.querySelector('.navbar-sub');
                if (subbar) {
                    subbarHeight = navbar.offsetHeight;
                    subbar.style.top = subbarHeight + 'px';
                }
                document.body.style.paddingTop = (navbar.offsetHeight + subbarHeight + 5) + 'px';
            }
        };
        this.setPadding();
        window.addEventListener('resize', this.setPadding);
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/Counter.ts":
/*!*************************************!*\
  !*** ./wwwroot/ts/share/Counter.ts ***!
  \*************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   Counter: () => (/* binding */ Counter)
/* harmony export */ });
class Counter {
    constructor() {
        this.SetCounter = () => {
            const counterElements = Array.from(document.getElementsByClassName('counter'));
            counterElements.forEach((element) => {
                const counterElement = element;
                if (!counterElement) {
                    return;
                }
                const incrementButtonElement = counterElement.querySelector('.counter_increment');
                const decrementButtonElement = counterElement.querySelector('.counter_decrement');
                const decrementNumberElement = counterElement.querySelector('.counter_number');
                const max = parseInt(counterElement.max) || 100;
                const min = parseInt(counterElement.min) || 0;
                let timer;
                let interval = 200; // 長押し時の間隔(ms)
                let step = 1;
                let longPressStep = 5;
                function changeValue(delta) {
                    let value = parseInt(decrementNumberElement.value) || 0;
                    value += delta;
                    value = Math.max(min, Math.min(max, value));
                    decrementNumberElement.value = String(value);
                }
                function handlePress(buttonElement, delta) {
                    buttonElement.addEventListener('mousedown', function () {
                        step = 1;
                        changeValue(delta);
                        timer = setTimeout(function repeat() {
                            step = longPressStep;
                            changeValue(delta * step);
                            timer = setTimeout(repeat, interval);
                        }, 500); // 0.5秒後に長押し判定
                    });
                    buttonElement.addEventListener('mouseup', function () {
                        clearTimeout(timer);
                    });
                    buttonElement.addEventListener('mouseleave', function () {
                        clearTimeout(timer);
                    });
                    buttonElement.addEventListener('touchend', function () {
                        clearTimeout(timer);
                    });
                }
                handlePress(incrementButtonElement, 1);
                handlePress(decrementButtonElement, -1);
            });
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/FetchApi.ts":
/*!**************************************!*\
  !*** ./wwwroot/ts/share/FetchApi.ts ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   FetchApi: () => (/* binding */ FetchApi)
/* harmony export */ });
var _a;
class FetchApi {
    constructor() {
        this.send = async (url, method, headers, body, responseKind = "json", needResponseData = true) => {
            let request;
            if (method == "GET") {
                request = new Request(url, {
                    method: method,
                    headers: headers
                });
            }
            else {
                request = new Request(url, {
                    method: method,
                    headers: headers,
                    body: JSON.stringify(body)
                });
            }
            ManageLoadElement.set();
            const response = await fetch(request).catch(e => { throw e; });
            if (!response.ok) {
                ManageLoadElement.remove();
                throw new Error(response.statusText);
            }
            if (!needResponseData) {
                ManageLoadElement.remove();
                return;
            }
            switch (responseKind) {
                case "json":
                    return await this.fetchResonse(response.json()).catch(e => { throw e; });
                case "text":
                    return await this.fetchResonse(response.text()).catch(e => { throw e; });
            }
        };
    }
    async fetchResonse(response) {
        return await response.then(data => {
            return data;
        })
            .catch(error => {
            // エラー処理
            console.log(error);
            throw error;
        })
            .finally(() => {
            ManageLoadElement.remove();
        });
    }
}
class ManageLoadElement {
}
_a = ManageLoadElement;
ManageLoadElement.loadElement = document.getElementById("loading-overlay");
ManageLoadElement.set = () => {
    if (_a.loadElement == null) {
        return;
    }
    _a.loadElement.classList.remove("nodisplay");
};
ManageLoadElement.remove = () => {
    if (_a.loadElement == null) {
        return;
    }
    _a.loadElement.classList.add("nodisplay");
};


/***/ }),

/***/ "./wwwroot/ts/share/LocalStrage.ts":
/*!*****************************************!*\
  !*** ./wwwroot/ts/share/LocalStrage.ts ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   LocalStrage: () => (/* binding */ LocalStrage)
/* harmony export */ });
class LocalStrage {
    static set(Obj_name, Obj) {
        window.sessionStorage.setItem(Obj_name, JSON.stringify(Obj));
    }
    static get(objName) {
        const obj = window.sessionStorage.getItem(objName);
        if (!this.check(objName)) {
            return false;
        }
        if (obj === null) {
            throw new Error("未知のエラー");
        }
        const parser = function (_k, v) { return v.toString().indexOf('function') === 0 ? eval('(' + v + ')') : v; };
        return JSON.parse(obj, parser);
    }
    static check(Obj_name) {
        if (window.sessionStorage.getItem(Obj_name)) {
            return true;
        }
        else {
            return false;
        }
    }
    static delete() {
        window.sessionStorage.clear();
    }
}


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
(() => {
/*!***************************************!*\
  !*** ./wwwroot/ts/OrderList/Index.ts ***!
  \***************************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/ControlNavbar */ "./wwwroot/ts/share/ControlNavbar.ts");
/* harmony import */ var _root_share_BackButton__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @root//share/BackButton */ "./wwwroot/ts/share/BackButton.ts");
/* harmony import */ var _GetOrderList__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./GetOrderList */ "./wwwroot/ts/OrderList/GetOrderList.ts");
/* harmony import */ var _SendOrder__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./SendOrder */ "./wwwroot/ts/OrderList/SendOrder.ts");




(async () => {
    new _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__.ControlNavbar();
    const getOrderList = new _GetOrderList__WEBPACK_IMPORTED_MODULE_2__.GetOrderList();
    const sendOrder = new _SendOrder__WEBPACK_IMPORTED_MODULE_3__.SendOrder();
    const backButton = new _root_share_BackButton__WEBPACK_IMPORTED_MODULE_1__.BackButton();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
    backButton.Buck();
})();

})();

/******/ })()
;
//# sourceMappingURL=OrderList.js.map