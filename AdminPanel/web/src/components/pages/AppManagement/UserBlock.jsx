import React, { useEffect, useState } from "react";
import { AmountBox, Button, Title } from "../../common.styled";
import DropDownComponent from "../../DropDownComponent/DropDownComponent";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";

import { toast } from 'react-toastify';

const UserBlock = () => {

  useEffect(() => {
    getallusersbalance();
  }, [])

  const [availableCU, setavailableCU] = useState(0);
  const [userBlockEnable, setUserBlockEnable] = useState("");
  const [userUnings, setUserUnings] = useState("");

  const [values, setValues] = useState({
    mobileNumber: "",
  });

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const userBlockDropDownItems = ["Enable", "Disable"];

  const userUningsDropDownItems = ["10%", "20%", "70%"];

  const handleSubmituser = async (e) => {
    e.preventDefault();

    const status = userBlockEnable === 'Enable' ? 'Y' : 'N';

    const data = {
      "mobile_number": values['mobileNumber'],
      "status": status
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/changestatususer`, {
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
          toast.success(res.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: undefined,
            theme: "dark",
          });

          setValues({
            mobileNumber: "",
          });

          setUserBlockEnable("");

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
        toast.success(err.message, {
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
  };

  const handleSubmitgamestatus = async (e) => {
    e.preventDefault();

    const data = {
      "config_key": "probability_status",
      "config_value": userUnings
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/updateconfig`, {
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
          toast.success(res.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: undefined,
            theme: "dark",
          });

          setUserUnings("");
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
  };

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
          setavailableCU(res.data.total_available);
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

  return (
    <form>
      <div>
        <h4 className={styles.title}>USER BLOCK </h4>

        <Title>MOBILE NUMBER</Title>
        <Input
          name="mobileNumber"
          value={values["mobileNumber"]}
          onChange={onChange}
        />

        <DropDownComponent
          title="ENABLE / DISABLE"
          items={userBlockDropDownItems}
          value={userBlockEnable}
          setValue={setUserBlockEnable}
        />
        <div className={styles.buttonContainer}>
          <Button onClick={handleSubmituser}>Process</Button>
        </div>
      </div>
      <div>
        <h4 className={styles.title}>SET PROBABILTY </h4>

        <DropDownComponent
          title="USER WINING ODDS"
          items={userUningsDropDownItems}
          value={userUnings}
          setValue={setUserUnings}
        />
        <div className={styles.buttonContainer}>
          <Button onClick={handleSubmitgamestatus}>Process</Button>
        </div>
      </div>
      <Title>TOTAL BALANCE - USERS</Title>
      <AmountBox>{availableCU}</AmountBox>
    </form>
  );
};

export default UserBlock;
