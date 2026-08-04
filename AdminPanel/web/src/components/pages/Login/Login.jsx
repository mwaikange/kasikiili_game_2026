import React, { useState } from "react";
import { Link } from "react-router-dom";
import Input from "../../Input/Input";

import styles from "./styles.module.css";

import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";

import { toast } from 'react-toastify';

const Login = ({ setLogin }) => {
  const [values, setValues] = useState({
    // mobile: "",
    email: "",
    password: "",
    // stateoftheassest: "",
  });
  const [navItem, setnavItem] = useState([]);

  const inputs = [
    // {
    //   type: "phone",
    //   name: "mobile",
    //   placeholder: "Mobile Number",
    // },
    {
      type: "email",
      name: "email",
      placeholder: "Email Address",
    },
    {
      type: "password",
      name: "password",

      placeholder: "Password",
    },
  ];

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const submitFunction = async (e) => {
    e.preventDefault();
    const body = {
      info: encrpty(JSON.stringify(values))
    }

    await fetch(`${config}/adminlogin`, {
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

          sessionStorage.setItem(`${document.location.hostname}`, res.data);

          getmoduledata();
          setTimeout(() => {
            setLogin(true);
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


  const getmoduledata = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/getmoduleaccess`, {
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
          const module = res.data.module;

          if (module.includes("Distributors Tab")) {
            navItem.push({
              icon: 'distributor',
              navItem: "DISTRIBUTORS",
              to: "/",
            })
          }

          if (module.includes("Users Tab")) {
            navItem.push({
              icon: 'user',
              navItem: "USERS",
              to: "/user",
            })
          }

          if (module.includes("App Management")) {
            navItem.push({
              icon: 'appmangement',
              navItem: "APP MANAGEMENT",
              to: "/appmanagement",
            })
          }

          sessionStorage.setItem(`${document.location.hostname}-access`, JSON.stringify(navItem));

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
            setnavItem([]);
            sessionStorage.setItem(`${document.location.hostname}-access`, JSON.stringify(navItem));
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

        sessionStorage.clear();

        setTimeout(() => {
          window.location.reload();
        }, 3000);
      });
  }

  return (
    <section className={styles.loginWrapper}>
      <div className={styles.logIn}>
        <h2 className={styles.title}>Login</h2>
        <p className={styles.text}>Login to manage payouts</p>
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
              Login
            </button>
          </div>
          <div to="#" className={styles.forgotPassword}>
            <span> Forgot your password?</span>
            {"  "}
            <Link to="/forgotPassword" className={styles.link}>
              {"  "}
              Click Here
            </Link>
          </div>
        </form>
      </div>
    </section>
  );
};

export default Login;
