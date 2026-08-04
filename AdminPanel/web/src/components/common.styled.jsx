import styled from "styled-components";

export const Container = styled.section`
  max-width: 1440px;
  width: 100%;
  margin: 0 auto;
  position: relative;

  padding-top: 150px;
  min-height: 100vh;
`;
export const Title = styled.p`
  font-family: ${({ fontFamily }) => (fontFamily ? fontFamily : "Poppins")};

  font-style: normal;
  font-weight: ${({ fontWeight }) => (fontWeight ? fontWeight : "700")};
  font-size: 16px;
  line-height: 150%;
  text-align: ${({ center }) => (center ? "center" : "")};
  color: #392e2d;
  padding: ${({ padding }) => (padding ? padding : "5px")};
  @media only screen and (max-width: 1399px) {
    font-size: ${({ mobileFontSize }) =>
      mobileFontSize ? mobileFontSize : "16px"};
  }
`;
export const AmountBox = styled.div`
  background: #1ea52b;
  border-radius: 14px;
  font-family: "Quicksand";
  font-style: normal;
  font-weight: 700;
  font-size: 36px;
  line-height: 100%;

  text-align: ${({ center }) => (center ? "center" : "right")};
  letter-spacing: 0.44px;
  color: #ffffff;
  padding: 8px 15px;

  min-width: ${({ minWidth }) => (minWidth ? minWidth : "100%")};
  @media only screen and (max-width: 1350px) {
    font-size: 28px;
  }
`;
export const Heading = styled.h2`
  font-family: "Poppins";
  font-style: normal;
  font-weight: 700;
  font-size: 32px;
  line-height: 150%;

  color: #392e2d;
`;

export const Tagline = styled.h4`
  font-family: "Poppins";
  font-style: normal;
  font-weight: 700;
  font-size: 26px;
  line-height: 150%;

  color: #392e2d;
`;

export const Button = styled.button`
  min-width: 20px;
  font-family: "Roboto";
  font-style: normal;
  font-weight: 700;
  font-size: 20px;
  line-height: 110%;
  text-align: center;
  color: #ffffff;
  background: #37266b;

  border-radius: 10px;
  padding: 12px 30px;
  margin-top: 20px;
  border: none;
  outline: none;
  cursor: pointer;
  width: ${({ width }) => (width ? width : "auto")};
  :active {
    box-shadow: 0px 0px 25px #7f55da;
  }
  @media only screen and (max-width: 1350px) {
    font-size: 16px;
  }
`;
