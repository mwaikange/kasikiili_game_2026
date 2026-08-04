import React, { useEffect, useState } from "react";
import { AmountBox, Heading, Tagline } from "../../../common.styled";
// import { userDetails } from "../../../../assets/data";
import { FaLongArrowAltLeft } from "react-icons/fa";
import styles from "./styles.module.css";
import { useParams, useNavigate } from "react-router-dom";
import { format } from 'date-fns';
import config from "../../../../config";
import { encrpty, decrpty } from "../../../../crypto";
import { toast } from 'react-toastify';

const UserDetails = () => {
  const { id } = useParams();

  useEffect(() => {
    getuserdata();
    getusertranscation();
  }, []);

  const navigate = useNavigate();
  const heading = [
    "DATE",
    "TRANS ID",
    "AMOUNTS(NS)",
    "DISTRIBUTORS",
    "ACTIVITY",
  ];

  const [data, setdata] = useState({
    mobile_number: "",
    full_name: "",
    email: "",
    region: "",
    balance: 0
  })

  const [tabledata, settabledata] = useState([]);

  const details = [
    // { key: "E-mail ", value: data.email },
    { key: "MOBILE ", value: data.mobile_number },
    { key: "REGION", value: data.region },
    { key: "UIN", value: data.UIN },
  ];


  const getuserdata = async () => {

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getuserdata/${id}`, {
      method: 'GET',
      headers: {
        'Content-type': 'application/json; charset=UTF-8',
        'authorization': `Bearer ${token}`
      },
    })
      .then((response) => response.json())
      .then((res) => {
        if (res.success) {
          res.data = JSON.parse(decrpty(res.data))[0];
          setdata(res.data);
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
            setdata([]);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        setdata([]);
      });
  }

  const getusertranscation = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getusertranscation/${id}`, {
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
          settabledata(res.data);
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
            settabledata([]);
          }
        }
      })
      .catch((err) => {
        console.log(err.message);
        settabledata([]);
      });
  }

  return (
    <div className={styles.distributorWrapper}>
      <FaLongArrowAltLeft
        className={styles.icon}
        onClick={() => navigate(-1)}
      />

      <div className={styles.distributorInfo}>
        <div className={styles.details}>
          {details.map((el, i) => (
            <Tagline className={styles.headingAndTagline} key={i}>
              {el.key} <span className={styles.clone}>:</span> {el.value}
            </Tagline>
          ))}
        </div>
        <div>
          <Tagline className={styles.tagline}>BALANCE</Tagline>
          <AmountBox center>{data.balance}</AmountBox>
        </div>
      </div>
      <h3 className={styles.historyTitle}>
        HISTORY : (Last fifteen (15) transactions only)
      </h3>
      <div className={styles.tableContainer}>
        {heading.map((el, i) => (
          <h3 className={styles.title} key={i}>
            {el}
          </h3>
        ))}
      </div>

      {tabledata.length>0 ? tabledata.map((el, i) => (
        <div
          className={`${styles.tableContainer} ${styles.tableContainer2}`}
          key={i}
        >
          <p className={`${styles.title} ${styles.value}`}>{format(new Date(el.trascantion_date), 'dd/MM/yyyy')}</p>{" "}
          <p className={`${styles.title} ${styles.value}`}>{el.transation_id}</p>
          <p className={`${styles.title} ${styles.value}`}>{el.amount}</p>
          <p className={`${styles.title} ${styles.value}`}>{el.distributor}</p>
          <p className={`${styles.title} ${styles.value}`}>{el.status}</p>
        </div>
      )) : <p className={`${styles.title}`} style={{'textAlign': 'center'}}>No Record found</p>}
    </div>
  );
};

export default UserDetails;
