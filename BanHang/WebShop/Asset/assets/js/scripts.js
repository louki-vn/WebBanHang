const searchWrapper = document.querySelector(".search-bar__form");
const inputBox = searchWrapper.querySelector("input");
const suggBox = searchWrapper.querySelector(".autocom-box");

// lấy dữ liệu
let suggestions =[]
$.ajax(
     {
          type: 'GET',
          url: '/Sales/ProductSales/GetPro',
          success: function (response) {
               $.each(response, function (key, item) {
                    suggestions.push(item.name);
               });
          }
     });

// tìm kiếm khi thực hiện sự kiện keyup
inputBox.onkeyup = (e) => {

     let userData = e.target.value; //user enetered data
     let emptyArray = [];
     if (userData) {
          emptyArray = suggestions.filter((data) => {
               //filtering array value and user characters to lowercase and return only those words which are start with user enetered chars
               data = data.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
               userData = userData.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
               return data.toLocaleLowerCase().includes(userData.toLocaleLowerCase());
          });
          emptyArray = emptyArray.map((data) => {
               // passing return data inside li tag
               return data = `<li>${data}</li>`;
          });
          searchWrapper.classList.add("active"); //show autocomplete box
          showSuggestions(emptyArray);
          let allList = suggBox.querySelectorAll("li");
          for (let i = 0; i < allList.length; i++) {
               //adding onclick attribute in all li tag
               allList[i].setAttribute("onclick", "select(this)");
          }
     } else {
          searchWrapper.classList.remove("active"); //hide autocomplete box
     }
}

function select(element) {
     let selectData = element.textContent;
     inputBox.value = selectData;
     searchWrapper.classList.remove("active");
}

function showSuggestions(list) {
     let listData;
     if (!list.length) {
          userValue = inputBox.value;
          listData = `<li>${userValue}</li>`;
     } else {
          listData = list.join('');
     }
     suggBox.innerHTML = listData;
}

