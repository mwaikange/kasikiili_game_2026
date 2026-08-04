import React, { useEffect, useState } from "react";
import { Title, Button, AmountBox } from "../../common.styled";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";
import { toast } from 'react-toastify';

const CreditGeneration = () => {

  useEffect(() => {
    getavailablecredit();
  }, [])

  const [availableC, setavailableC] = useState(0);
  const [valuesC, setValuesC] = useState({
    creditGenerationamount: ""
  });

  const [values, setValues] = useState({
    distributorcode: "",
    creditTransferAmount: "",
  });

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const onChangeC = (e) => {
    setValuesC({ ...valuesC, [e.target.name]: e.target.value });
  };


  const handlecreditSubmit = async (e) => {
    e.preventDefault();

    const data = {
      "amount": valuesC['creditGenerationamount']
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/generatebalance`, {
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

          setValuesC({
            creditGenerationamount: ""
          });
          getavailablecredit();
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

  const handleSubmit = async (e) => {
    e.preventDefault();

    const data = {
      "amount": values['creditTransferAmount'],
      "distributor_code": values['distributorcode']
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/transfertodistri`, {
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
            creditTransferAmount: "",
            distributorcode: ""
          });

          getavailablecredit();
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

  const getavailablecredit = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getavailablecredit`, {
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
          setavailableC(res.data.available_credit);
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
        toast.error(TypeError.message, {
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
      <h3 className={styles.title}>CREDIT GENERATION</h3>
      <div className={styles.generateAmount}>
        <Title>TYPE AMOUNT</Title>
        <Input
          type="text"
          name="creditGenerationamount"
          value={valuesC["creditGenerationamount"]}
          onChange={onChangeC}
        />
        <Button onClick={handlecreditSubmit}>GENERATE</Button>
      </div>
      <div className={styles.availableContainer}>
        <h3 className={styles.title}>AVAILABLE CREDITS</h3>
        <AmountBox>{availableC}</AmountBox>
      </div>
      <h3 className={styles.title}>CREDIT TRANSFERS</h3>
      <div className={styles.inputContainer}>
        <Title padding="0 0">DISTRIBUTOR CODE</Title>
        <Input
          type="text"
          name="distributorcode"
          value={values["distributorcode"]}
          onChange={onChange}
        />
      </div>{" "}
      <div className={styles.inputContainer}>
        <Title padding="0 0">TYPE AMOUNT </Title>
        <Input
          type="text"
          name="creditTransferAmount"
          value={values["creditTransferAmount"]}
          onChange={onChange}
        />
      </div>
      <Button onClick={handleSubmit}>TRANSFER</Button>
    </form>
  );
};

export default CreditGeneration;
