import React, { useState } from "react";
import { Button, Title } from "../../common.styled";
import DropDownComponent from "../../DropDownComponent/DropDownComponent";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";

import { toast } from 'react-toastify';

const CreditDistributor = () => {
  const [region, setRegion] = useState("");
  const [values, setValues] = useState({
    mobilenumber: "",
    email: "",
    surname: "",
    name: "",
  });

  const [valuesD, setValuesD] = useState({
    distributorCode: "",
  });

  const [customstyle, setcustomstyle] = useState(true);

  const inputs = [
    {
      title: "MOBILE NUMBER",
      type: "text",
      name: "mobilenumber",
    },
    {
      title: "EMAIL",
      type: "email",
      name: "email",
    },
    {
      title: "SURNAME",
      type: "text",
      name: "surname",
    },
    {
      title: "NAME",
      type: "text",
      name: "name",
    },
  ];

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const onChangeD = (e) => {
    setValuesD({ ...valuesD, [e.target.name]: e.target.value });
  };

  const dropDownItems = [
    "Zambezi",
    "Erongo",
    "Hardap",
    "Karas",
    "Kavango West",
    "Kavango East",
    "Khomas",
    "Kunene",
    "Ohangwena",
    "Omaheke",
    "Otjozondjupa",
    "Oshikoto",
    "Oshana",
    "Omusati"
  ];

  const handleSubmit = async (e) => {
    e.preventDefault();

    const data = {
      mobile_number: values['mobilenumber'],
      email: values['email'],
      name: values['name'],
      surname: values['surname'],
      region: region
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/creatdistributor`, {
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
            mobilenumber: "",
            email: "",
            surname: "",
            name: ""
          });
          setRegion("");
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

  const handleSubmitDistributor = async (e) => {
    e.preventDefault();

    const data = {
      distributor_code: valuesD['distributorCode']
    }

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(data))
    }

    await fetch(`${config}/resetpassworddistributor`, {
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

          setValuesD({
            distributorCode: ""
          });

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

  return (
    <form action="">
      <h4 className={styles.title}>CREATE DISTRIBUTOR </h4>

      <div>
        {inputs.map((el, i) => (
          <div key={i}>
            <Title>{el.title}</Title>
            <Input {...el} value={values[el.name]} onChange={onChange} />
          </div>
        ))}
        <DropDownComponent
          title="REGION"
          items={dropDownItems}
          value={region}
          setValue={setRegion}
          customstyle={customstyle}
        />
        <Button onClick={(e) => handleSubmit(e)}>CREATE</Button>
        <div className={styles.inputContainer}>
          <Title>DISTRIBUTOR CODE</Title>
          <Input
            name="distributorCode"
            value={values["distributorCode"]}
            type="text"
            onChange={onChangeD}
          ></Input>
        </div>
        <Button onClick={(e) => handleSubmitDistributor(e)}>Reset Distro Pass</Button>
      </div>
    </form>
  );
};

export default CreditDistributor;
