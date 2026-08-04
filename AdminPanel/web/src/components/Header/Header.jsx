import React, { useEffect } from "react";
import styles from "./styles.module.css";
import "./style.css";
import { logout } from "../../assets/image/images";
import config from "../../config";
import { encrpty, decrpty } from "../../crypto";
import { toast } from 'react-toastify';

const Header = ({ login, setLogin }) => {

  useEffect(() => {
    const token = sessionStorage.getItem(`${document.location.hostname}`);

    if (token) {
      setLogin(true);
    } else {
      setLogin(false);
    }
  }, []);

  const adminlogout = async (e) => {
    e.preventDefault();

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    await fetch(`${config}/logout`, {
      method: 'POST',
      body: JSON.stringify({}),
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
          sessionStorage.clear();

          setTimeout(() => {
            setLogin(false);
          }, 3000);
        } else {
          toast.success("Logout successfully", {
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
            setLogin(false);
          }, 3000);
        }
      })
      .catch((err) => {
        console.log(err.message);
        toast.success("Logout successfully", {
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
            setLogin(false);
          }, 3000);
      });
  }

  return (
    <section className={styles.headerWrapper}>
      <header className={styles.header}>
        <div className={styles.logoContainer}>
          {" "}
          {login && (
            <img
              src={logout}
              alt="#"
              className={styles.logOut}
              onClick={adminlogout}
              title="Logout"
            />
          )}
          <h2 className={styles.logo}>KASIKILI</h2>
          <p className={styles.tagline}>DIGITAL BERGMANN JACKPOTS</p>
        </div>
        <div className={styles.barContainer}>
          {[...new Array(4)].map((_, i) => (
            <div
              key={i}
              className={`bar${i + 1} ${styles.bar} ${i % 2 === 0 ? styles.evenBar : styles.oddBar
                }`}
            ></div>
          ))}
        </div>
      </header>
    </section>
  );
};

export default Header;
