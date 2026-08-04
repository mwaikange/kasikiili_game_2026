import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import Input from "../../Input/Input";
import styles from "./styles.module.css";
import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";
import { toast } from 'react-toastify';

const ForgotPassword = ({ setLogin }) => {

  const navigate = useNavigate();

  const [values, setValues] = useState({
    // mobile: "",
    email: "",
    // staffcode: "",
  });

  const inputs = [
    // {
    //   type: "phone",
    //   name: "mobile",
    //   placeholder: "John Doe",
    // },
    {
      type: "email",
      name: "email",
      placeholder: "Email Address",
    },
    // {
    //   type: "text",
    //   name: "staffcode",

    //   placeholder: "Staff Code",
    // },
  ];
  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const submitFunction = async (e) => {
    e.preventDefault();

    values['user_type'] = "Admin";
    console.log("values", values);

    const body = {
      info: encrpty(JSON.stringify(values))
    }

    await fetch(`${config}/forgetPassword`, {
      method: 'POST',
      body: JSON.stringify(body),
      headers: {
        'Content-type': 'application/json; charset=UTF-8',
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

          setTimeout(() => {
            navigate('/login');
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
    <section className={styles.loginWrapper}>
      <div className={styles.logIn}>
        <h2 className={styles.title}>Forget Password</h2>
        <p className={styles.text}>Enter E-mail</p>
        <form className={styles.form}>
          {" "}
          <div className={styles.inputContainer}>
            {inputs.map((input, i) => (
              <div className={styles.inputWrapper} key={i}>
                <Input
                  {...input}
                  value={values[input.name]}
                  onChange={onChange}
                />
              </div>
            ))}
          </div>
          <div className={styles.buttonContainer}>
            <button
              type="submit"
              className={styles.button}
              onClick={(e) => submitFunction(e)}
            >
              RESET
            </button>
          </div>
          <div to="#" className={styles.forgotPassword}>
            <span> Back to Login ? </span>
            {"  "}
            <Link to="/login" className={styles.link}>
              {"  "}
              Click Here
            </Link>
          </div>
        </form>
      </div>
    </section>
  );
};

export default ForgotPassword;
