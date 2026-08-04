import React, { useEffect, useState } from "react";
import { AmountBox, Button, Title } from "../../common.styled";
import DropDownComponent from "../../DropDownComponent/DropDownComponent";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";

import { toast } from 'react-toastify';

const DistributorBlock = () => {

  useEffect(() => {
    getalldistributorbalance();
  }, [])

  const [availableCD, setavailableCD] = useState(0);
  const [mobileNumber, setMobileNumber] = useState("");
  const [distorBlockEnable, setDistorBlockEnable] = useState("");
  const [gameStatusEnable, setgameStatusEnable] = useState("");
  const [values, setValues] = useState({
    mobileNumber: "",
  });

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };
  const distorBlockDrropDownItems = ["Enable", "Disable"];
  const gameStatusDownItems = ["Enable", "Disable"];

  const handleSubmitDistributor = async (e) => {
    e.preventDefault();

    const status = distorBlockEnable === 'Enable' ? 'Y' : 'N';

    const data = {
      "mobile_number": values['mobileNumber'],
      "status": status
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/changestatusdistributor`, {
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

          setDistorBlockEnable("");
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

  const handleSubmitgamestatus = async (e) => {
    e.preventDefault();

    const status = gameStatusEnable === 'Enable' ? 1 : 0;

    const data = {
      "config_key": "game_status",
      "config_value": status
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
          toast.success("Game status updated successfully", {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: undefined,
            theme: "dark",
          });

          setgameStatusEnable("");
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
          setavailableCD(res.data.total_available);
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
        <h4 className={styles.title}>DISTRIBUTOR BLOCK </h4>

        <Title>MOBILE NUMBER</Title>
        <Input
          name="mobileNumber"
          value={values["mobileNumber"]}
          onChange={onChange}
        />

        <DropDownComponent
          title="ENABLE / DISABLE"
          items={distorBlockDrropDownItems}
          value={distorBlockEnable}
          setValue={setDistorBlockEnable}
        />
        <div className={styles.buttonContainer}>
          <Button onClick={handleSubmitDistributor}>Process</Button>
        </div>
      </div>
      <div>
        <h4 className={styles.title}>GAME STATUS </h4>

        <DropDownComponent
          title="ENABLE / DISABLE"
          items={gameStatusDownItems}
          value={gameStatusEnable}
          setValue={setgameStatusEnable}
        />
        <div className={styles.buttonContainer}>
          <Button onClick={handleSubmitgamestatus}>Process</Button>
        </div>
      </div>
      <Title>TOTAL BALANCE - DISTRIBUTORS</Title>
      <AmountBox>{availableCD}</AmountBox>
    </form>
  );
};

export default DistributorBlock;
