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
/* harmony import */ var _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/LocalStrage */ "./wwwroot/ts/share/LocalStrage.ts");
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");


class GetOrderList {
    constructor() {
        this.setOrderList = () => {
            let orderEntityList = [];
            if (_root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__.LocalStrage.check("OrderEntityList")) {
                orderEntityList = _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__.LocalStrage.get("OrderEntityList");
            }
            this.send(orderEntityList).then((data) => {
                document.getElementById("order_list").innerHTML = data;
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


class SendOrder {
    constructor() {
        this.setOrderList = () => {
            document.getElementById("send_order")?.addEventListener("click", async () => {
                let orderEntityList = [];
                if (_root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__.LocalStrage.check("OrderEntityList")) {
                    orderEntityList = _root_share_LocalStrage__WEBPACK_IMPORTED_MODULE_0__.LocalStrage.get("OrderEntityList");
                }
                this.send(orderEntityList).then((data) => {
                    window.alert("注文を送信しました");
                });
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

/***/ "./wwwroot/ts/share/FetchApi.ts":
/*!**************************************!*\
  !*** ./wwwroot/ts/share/FetchApi.ts ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   FetchApi: () => (/* binding */ FetchApi)
/* harmony export */ });
class FetchApi {
    constructor() {
        this.send = async (url, method, headers, body, 
        //body: any,
        responseKind = "json", needResponseData = true) => {
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
            const response = await fetch(request).catch(e => { throw e; });
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            if (!needResponseData) {
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
        });
    }
}


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
            throw new Error("���m�̃G���[");
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
/* harmony import */ var _GetOrderList__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./GetOrderList */ "./wwwroot/ts/OrderList/GetOrderList.ts");
/* harmony import */ var _SendOrder__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./SendOrder */ "./wwwroot/ts/OrderList/SendOrder.ts");


(async () => {
    const getOrderList = new _GetOrderList__WEBPACK_IMPORTED_MODULE_0__.GetOrderList();
    const sendOrder = new _SendOrder__WEBPACK_IMPORTED_MODULE_1__.SendOrder();
    getOrderList.setOrderList();
    sendOrder.setOrderList();
})();

})();

/******/ })()
;
//# sourceMappingURL=OrderList.js.map