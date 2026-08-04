import React, { useEffect, useState } from "react";
import { AmountBox, Title } from "../../common.styled";
import DropDownComponent from "../../DropDownComponent/DropDownComponent";
import SelectDropDowncomponent from "../../SelectDropDowncomponent/SelectDropDowncomponent";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import Table from "./Table/Table";
import MyDatePicker from "../../DatePicker/DatePicker";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";
import * as FileSaver from 'file-saver';
import XLSX from 'sheetjs-style';
import { toast } from 'react-toastify';

const Distributor = () => {

  useEffect(() => {
    getdistributorlist();
    getalldistributor();
    getalldistributorbalance();
  }, []);

  const [total_balance, settotalbalance] = useState(0);
  const [total_distributor, setdistributor] = useState(0);
  const [criteria, setCriteria] = useState("");
  const [searchValue, setSearchValue] = useState("");
  const [startDate, setStartDate] = useState(new Date('12/31/1999'));
  const [endDate, setEndDate] = useState(new Date().setDate(new Date().getDate() + 1));
  // const [filtervalue, setfiltervalue] = useState({
  //   filter: '',
  //   search: ''
  // });

  const [distributorData, setdistributorData] = useState([]);
  const [exceldata, setexceldata] = useState([]);
  const [dropstatus, setdropstatus] = useState(false);

  const dropDownItems = [
    "Mobile no",
    "Status",
    "Region",
    // "Request Status",
    "List From Highest Balance",
    "List From Lowest Balance",
  ];

  const dropDownstatus = [
    "Enabled",
    "Disabled"
  ];

  const data = [
    { title: "TOTAL  BALANCE - DISTRIBUTORS", value: total_balance },
    { title: "NUMBER OF  DISTRIBUTORS", value: total_distributor },
  ];

  const searchFunction = (e) => {
    setSearchValue(e.target.value);
    // setfiltervalue({
    //   "filter": criteria,
    //   "search": e.target.value
    // })
  };

  const getalldistributorbalance = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getalldistributorbalance`, {
      method: 'GET',
      headers: {
        'Content-type': 'application/json; charset=UTF-8',
        'authorization': `Bearer ${token}`
      },
    })
      .then((response) => response.json())
      .then((res) => {
        if (res.success) {
          res.data = JSON.parse(decrpty(res.data));
          settotalbalance(res.data.total_available);
        } else {
          if (res.success_code === 401) {
            toast.error(res.message, {
              position: "top-right",
              autoClose: 3000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: false,
              progress: undefined,
              theme: "dark",
            });
            sessionStorage.clear();

            setTimeout(() => {
              window.location.reload();
            }, 3000);
          } else {
            settotalbalance(0);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settotalbalance(0);
      });
  }

  const getalldistributor = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getalldistributor`, {
      method: 'GET',
      headers: {
        'Content-type': 'application/json; charset=UTF-8',
        'authorization': `Bearer ${token}`
      },
    })
      .then((response) => response.json())
      .then((res) => {
        if (res.success) {
          res.data = JSON.parse(decrpty(res.data));
          setdistributor(res.data.total_distributors);
        } else {
          if (res.success_code === 401) {
            toast.error(res.message, {
              position: "top-right",
              autoClose: 3000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: false,
              progress: undefined,
              theme: "dark",
            });
            sessionStorage.clear();

            setTimeout(() => {
              window.location.reload();
            }, 3000);
          } else {
            toast.error(res.message, {
              position: "top-right",
              autoClose: 3000,
              hideProgressBar: false,
              closeOnClick: true,
              pauseOnHover: true,
              draggable: false,
              progress: undefined,
              theme: "dark",
            });
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        toast.error(err.message, {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: false,
          progress: undefined,
          theme: "dark",
        });
      });
  }

  const handleSearch = () => {
    getdistributorlist();
  };


  const getdistributorlist = async () => {
    const data = {
      "filtervalue": {
        filter: criteria,
        search: searchValue
      },
      "filtervalue2": {
        filter: 'daterange',
        from: startDate,
        to: endDate
      }
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/getdistributorlist`, {
      method: 'POST',
      body: JSON.stringify(body),
      headers: {
        'Content-type': 'application/json; charset=UTF-8',
        'authorization': `Bearer ${token}`
      },
    })
      .then((response) => response.json())
      .then((res) => {
        if (res.success) {
          res.data = JSON.parse(decrpty(res.data));
          setdistributorData(res.data.ditributordata);
          setexceldata(res.data.exceldata);
        } else {
          setdistributorData([]);
          setexceldata([]);
        }
      })
      .catch((err) => {
        console.log(err.message);
      });
  }

  const handledownlod = async () => {
    const date = new Date();
    const filetype = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const filename = "distributor_" + date.getDate() + "_" + (date.getMonth() + 1) + ".xlsx";

    const ws = XLSX.utils.json_to_sheet(exceldata);
    const wb = { Sheets: {'distributors': ws}, SheetNames: ['distributors']};
    const excelbuffer = XLSX.write(wb, { bookType: "xlsx", type: 'array'})
    const data = new Blob([excelbuffer], { type: filetype})
    FileSaver.saveAs(data, filename);
  }

  const changedrop = (e) => {
    e.preventDefault();
    const data = e.target.value;

    setCriteria(data);
    setSearchValue('');
    if(data === 'Status' || data === 'Request Status') {
      setdropstatus(true);
    } else {
      setdropstatus(false);
    }
  }

  return (
    <div>
      <div className={styles.searchDatesTotal}>
        <div className={`${styles.search} `}>
          <div className={styles.contentBox}>
          <SelectDropDowncomponent
              title="Search Criteria"
              items={dropDownItems}
              value={criteria}
              setValue={setCriteria}
              onChange={changedrop}
            />
            {dropstatus === true ? <SelectDropDowncomponent
              title="Select status"
              items={dropDownstatus}
              value={searchValue}
              onChange={searchFunction}
            /> :
              <Input
                name="search"
                placeholder="Type reference"
                label="Search Reference"
                value={searchValue}
                onChange={searchFunction}
              />
            }
          </div>
          <button className={styles.searchButton} onClick={handleSearch}>
            Search
          </button>
          <button disabled={exceldata.length === 0} className={styles.downLoadExcelButton} onClick={handledownlod}>Download Excel</button>
        </div>

        <div>
          <MyDatePicker
            label="Date From"
            date={startDate}
            setDate={setStartDate}
            minDate={new Date('12/31/1999')}
          />

          <MyDatePicker date={endDate} setDate={setEndDate} minDate={startDate} label="Date To" />
        </div>
        <div className={styles.TotalAmout}>
          {data.map((el, i) => (
            <div key={i} className={styles.totalAmount}>
              <Title>{el.title}</Title>
              <AmountBox minWidth="240px">{el.value}</AmountBox>
            </div>
          ))}
        </div>
      </div>{" "}
      <Table tabledata={distributorData} />
    </div>
  );
};

export default Distributor;
