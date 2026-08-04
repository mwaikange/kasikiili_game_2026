import React, { useEffect, useState } from "react";
import { AmountBox, Title } from "../../common.styled";
import DropDownComponent from "../../DropDownComponent/DropDownComponent";
import SelectDropDowncomponent from "../../SelectDropDowncomponent/SelectDropDowncomponent";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import SuccessPopup from "./SuccessPopup/TickUpdatePopup";
import Table from "./Table/Table";
import MyDatePicker from "../../DatePicker/DatePicker";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";
import * as FileSaver from 'file-saver';
import XLSX from 'sheetjs-style';
import { toast } from 'react-toastify';

const User = () => {

  useEffect(() => {
    getalluserdata();
    getallusers();
    getallusersbalance();
    getallcashrequsers();
    getallcashreq();
  }, [])

  const [total_balance, settotal_balance] = useState(0);
  const [total_users, settotal_users] = useState(0);
  const [total_cash_req, settotal_cash_req] = useState(0);
  const [total_cash_out, settotal_cash_out] = useState(0);

  const [startDate, setStartDate] = useState(new Date('12/31/1999'));
  const [endDate, setEndDate] = useState(new Date().setDate(new Date().getDate() + 1));
  const [criteria, setCriteria] = useState("");
  const [searchValue, setSearchValue] = useState("");
  const dropDownItems = [
    "Mobile no",
    "Status",
    "Region",
    "Request Status",
    "List From Highest Balance",
    "List From Lowest Balance",
  ];

  const dropDownstatus = [
    "Enabled",
    "Disabled"
  ];

  const dropDownReqstatus = [
    "Cashout Request",
    "Cashout not Request"
  ];

  const [file, setFile] = useState("");
  const [tickUpdate, setTickUpdate] = useState(false);
  const [massUpdate, setMassUpdate] = useState(false);
  const [userdata, setuserdata] = useState([]);
  const [exceldata, setexceldata] = useState([]);
  const [filtervalue, setfiltervalue] = useState({
    filter: '',
    search: ''
  });

  const [usertick, setUsertick] = useState([]);
  const [exceljsondata, setexeljsondata] = useState([]);
  const [dropstatus, setdropstatus] = useState(false);
  const [reqdropstatus, setreqdropstatus] = useState(false);

  const searchFunction = (e) => {
    setSearchValue(e.target.value);
    // setfiltervalue({
    //   "filter": criteria,
    //   "search": e.target.value
    // })
  };

  const data = [
    { title: "TOTAL  BALANCE - USERS", value: total_balance },
    { title: "NUMBER OF  REGISTERED USERS", value: total_users },
    { title: "TOTAL CASH OUT REQUEST  - N$", value: total_cash_req },
    { title: "TOTAL USERS CASHING OUT ", value: total_cash_out },
  ];

  const handleSearch = (e) => {
    e.preventDefault();

    getalluserdata();
  };

  const getallusers = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getallusers`, {
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
          settotal_users(res.data.total_users);
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
            settotal_balance(0);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settotal_users(0);
      });
  }

  const getallusersbalance = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getallusersbalance`, {
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
          settotal_balance(res.data.total_available);
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
            settotal_balance(0);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settotal_balance(0);
      });
  }

  const getallcashrequsers = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getallcashrequsers`, {
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
          settotal_cash_out(res.data.total_cashout_user);
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
            settotal_cash_out(0);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settotal_cash_out(0);
      });
  }

  const getallcashreq = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getallcashreq`, {
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
          settotal_cash_req(res.data.total_cashout_balance == null ? 0 : res.data.total_cashout_balance);
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
            settotal_cash_req(0);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settotal_cash_req(0);
      });
  }

  const getalluserdata = async () => {
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

    await fetch(`${config}/getallusers`, {
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
          setuserdata(res.data.userdata);
          setexceldata(res.data.exceldata);
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
            setuserdata([]);
            setexceldata([]);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        setuserdata([]);
        setexceldata([]);
      });
  }

  const handledownlod = async () => {
    const date = new Date();
    const filetype = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const filename = "user_" + date.getDate() + "_" + (date.getMonth() + 1) + ".xlsx";

    const ws = XLSX.utils.json_to_sheet(exceldata);
    const wb = { Sheets: { 'users': ws }, SheetNames: ['users'] };
    const excelbuffer = XLSX.write(wb, { bookType: "xlsx", type: 'array' })
    const data = new Blob([excelbuffer], { type: filetype })
    FileSaver.saveAs(data, filename);
  }

  const getdata = (user_id) => {
    const index = usertick.indexOf(user_id);
    setexeljsondata([]);

    if (index > -1) {
      usertick.splice(index, 1);
    } else {
      usertick.push(user_id);
    }
  }

  const acceptreq = async () => {
    if (usertick.length > 0) {
      for (let dat of usertick) {
        const data = {
          req_id: dat
        }
        const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

        const body = {
          info: encrpty(JSON.stringify(data))
        }

        await fetch(`${config}/acceptcashout`, {
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

            } else {
              if (res.success_code === 401) {
                sessionStorage.clear();

                setTimeout(() => {
                  window.location.reload();
                }, 3000);
                return false;
              } else {
                return false;
              }
            }
          })
          .catch((err) => {
            return false;
          });
      }

      setUsertick([]);
      getalluserdata();
      getallusers();
      getallusersbalance();
      getallcashrequsers();
      getallcashreq();

      toast.success("Request updated successfully", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: false,
        progress: undefined,
        theme: "dark",
      });
    } else {
      toast.error("Select any one data", {
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

  const massselect = async (e) => {
    e.preventDefault();
    setUsertick([]);

    for (let dat of exceljsondata) {
      if (dat['REQUEST'] != '-' && dat['REQUEST'] != null && dat['REQUEST'] != undefined) {
        usertick.push(dat['REQUEST']);
      }
    }

    if (usertick.length > 0) {
      acceptreq();
    } else {
      toast.error("There are no cash out requests in this file excel.", {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: false,
        progress: undefined,
        theme: "dark",
      });

      setFile('');
      setUsertick([]);
      setexeljsondata([]);
    }
  }

  const readUploadFile = (e) => {
    e.preventDefault();

    if (e.target.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
      if (e.target.files) {
        setFile(e.target.files[0].name);

        const reader = new FileReader();
        reader.onload = (e) => {
          const data = e.target.result;
          const workbook = XLSX.read(data, { type: "array" });
          const sheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[sheetName];
          const json = XLSX.utils.sheet_to_json(worksheet);
          setexeljsondata(json);
        };
        reader.readAsArrayBuffer(e.target.files[0]);
      }
    } else {
      toast.error('Please select valid excel file', {
        position: "top-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: false,
        progress: undefined,
        theme: "dark",
      })

      setFile('');
      setexeljsondata([]);
    }
  }

  const changedrop = (e) => {
    e.preventDefault();
    const data = e.target.value;

    setCriteria(data);
    setSearchValue('');
    if (data === 'Status' || data === 'Request Status') {
      setdropstatus(true);
      if (data === 'Status') {
        setreqdropstatus(false);
      } else {
        setreqdropstatus(true);
      }
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
              items={reqdropstatus ? dropDownReqstatus : dropDownstatus}
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

        <div className={styles.details}>
          <div>
            <MyDatePicker
              label="Date From"
              date={startDate}
              setDate={setStartDate}
              minDate={new Date('12/31/1999')}
            />

            <MyDatePicker date={endDate} setDate={setEndDate} minDate={startDate} label="Date To" />
          </div>

          <div className={styles.updateBox}>
            <label htmlFor="file" className={styles.fileLabel}>
              <input
                type="file"
                id="file"
                className={styles.fileInput}
                accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
                onChange={readUploadFile}
              />
              <p className={styles.fileText}>Choose File</p>
              {file !== '' ? <p className={styles.fileTextname}>{file}</p> : ''}
            </label>
            <div className={styles.buttonContainer}>
              <button
                className={styles.upDateButton}
                // onClick={() => setMassUpdate((prev) => !prev)}
                onClick={massselect}
              >
                MASS UPDATE
              </button>{" "}
              <button
                // onClick={() => setTickUpdate((prev) => !prev)}
                onClick={acceptreq}
                className={styles.upDateButton}
              >
                TICK UPDATE
              </button>
            </div>
            {/* <Title>35 SUCCESSFULL UPDATES !!</Title> */}
          </div>
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
      <Table tabledata={userdata} getdata={getdata} />
      {tickUpdate && <SuccessPopup setModal={setTickUpdate} />}
      {massUpdate && <SuccessPopup setModal={setMassUpdate} />}
    </div>
  );
};

export default User;
