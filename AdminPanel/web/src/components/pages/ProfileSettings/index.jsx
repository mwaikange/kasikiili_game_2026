import React, { useEffect, useState } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";
import { Button, Title } from "../../common.styled";
import Input from "../../Input/Input";
import CheckBox from "./CheckBox/Checkbox";
import styles from "./styles.module.css";

import config from "../../../config";
import { encrpty, decrpty } from "../../../crypto";

import { toast } from 'react-toastify';

const ProfileSetting = () => {

  useEffect(() => {
    getprofile();
    getmoduleaccess();
  }, [])

  const navigate = useNavigate();

  const [profile, setProfile] = useState({
    name: '',
    surname: '',
    mobile_number: '',
    email: ''
  });
  const [fullname, setfullname] = useState('');
  const [module, setmodule] = useState('');
  const [userTab, setUserTab] = useState(false);
  const [distributorTab, setDistributorTab] = useState(false);
  const [appManagement, setAppManagement] = useState(false);
  const [values, setValues] = useState({
    name: "",
    surname: "",
    email: "",
    mobile_number: "",
  });

  const [user_type, setuser_type] = useState(4);

  const [passvalues, setpassValues] = useState({
    oldpassword: "",
    newpassword: "",
  });

  const headingData = [
    { key: "ADMIN NAME:", value: fullname },
    { key: "TAB ACCESS:", value: module },
  ];
  const passwordInputs = [
    {
      type: "password",
      placeholder: "Old Password",
      name: "oldpassword",
    },
    {
      type: "password",
      placeholder: "New Password",
      name: "newpassword",
    },
  ];
  const detailsInput = [
    {
      type: "text",
      placeholder: "Name",
      name: "name",
    },
    {
      type: "text",
      placeholder: "SURNAME",
      name: "surname",
    },
    {
      type: "email",
      placeholder: "Email",
      name: "email",
    },
    {
      type: "text",
      placeholder: "MOBILE NUMBER",
      name: "mobile_number",
    },
  ];

  const onChangepass = (e) => {
    setpassValues({ ...passvalues, [e.target.name]: e.target.value });
  };

  const onChange = (e) => {
    setValues({ ...values, [e.target.name]: e.target.value });
  };

  const passwordsubmit = async (e) => {
    e.preventDefault();

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(passvalues))
    }

    await fetch(`${config}/changePassword`, {
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
          sessionStorage.clear();

          setTimeout(() => {
            window.location.reload();
          }, 3000);
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

  const handleSubmit = async (e) => {
    e.preventDefault();
    let module_data = [];
    if (userTab) {
      module_data.push(1);
    }
    if (distributorTab) {
      module_data.push(2);
    }
    if (appManagement) {
      module_data.push(3);
    }

    values['module_data'] = module_data;

    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    const body = {
      info: encrpty(JSON.stringify(values))
    }

    await fetch(`${config}/createadmin`, {
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
            name: "",
            surname: "",
            email: "",
            mobile_number: ""
          });

          setUserTab(false);
          setDistributorTab(false);
          setAppManagement(false);
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

  const getprofile = async () => {
    const token = JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).accesstoken;

    setuser_type(JSON.parse(decrpty(sessionStorage.getItem(`${document.location.hostname}`))).user_type);

    await fetch(`${config}/getprofiledata`, {
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
          setProfile(res.data[0]);
          setfullname(res.data[0].full_name);
          // test
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

  const getmoduleaccess = async () => {
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
          setmodule(res.data.module.join(', '))
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
    <div>
      {" "}
      <div className={styles.header}>
        {headingData.map((el, i) => (
          <p className={styles.keyValue} key={i}>
            <span>{el.key}</span>
            <span className={styles.value}>{el.value}</span>
          </p>
        ))}
      </div>
      <h2 className={styles.heading}>PROFILE SETTINGS</h2>
      <div className={styles.infoContainer}>
        <div className={styles.keyContainer}>
          <Title>Name</Title> <Title>SURNAME</Title>{" "}
          <Title>EMAIL ADDRESS</Title> <Title>Mobile Number</Title>
        </div>
        <div>
          <Title fontWeight="500" fontFamily="'Quicksand'">
            {profile.name === '' ? '-' : profile.name}
          </Title>{" "}
          <Title fontWeight="500" fontFamily="'Quicksand'">
            {profile.surname === '' ? '-' : profile.surname}
          </Title>{" "}
          <Title fontWeight="500" fontFamily="'Quicksand'">
            {profile.email === '' ? '-' : profile.email}
          </Title>{" "}
          <Title fontWeight="500" fontFamily="'Quicksand'">
            +264 {profile.mobile_number === '' ? '-' : profile.mobile_number}
          </Title>
        </div>
      </div>
      <form className={styles.inputWrapper}>
        <div>
          {passwordInputs.map((el, i) => (
            <div className={styles.inputContainer} key={i}>
              <Input {...el} value={passvalues[el.name]} onChange={onChangepass} />
            </div>
          ))}
          <Button onClick={passwordsubmit} width="100%" type="submit">
            CHANGE PASSWORD
          </Button>
        </div>
        {user_type === 4 ? '' :
          <>
            <div>
              {detailsInput.map((el, i) => (
                <div className={styles.inputContainer} key={i}>
                  <Input {...el} value={values[el.name]} onChange={onChange} />
                </div>
              ))}
            </div>
            <div>
              <h4 className={styles.title}>LEVEL ( TAB) ACCESS</h4>
              <div className={styles.checkboxContainer}>
                <CheckBox
                  checked={userTab}
                  setChecked={setUserTab}
                  label="USER  TAB" />
                <CheckBox
                  label="DISTRIBUTOR  TAB"
                  checked={distributorTab}
                  setChecked={setDistributorTab} />
                <CheckBox
                  label="APP MANAGEMENT"
                  checked={appManagement}
                  setChecked={setAppManagement} />
              </div>
              <Button onClick={handleSubmit} width="100%">
                CREATE STAFF
              </Button>
            </div>
          </>
        }
      </form>
    </div>
  );
};

export default ProfileSetting;
