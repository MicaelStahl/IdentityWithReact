import React, { Component } from "react";
import { connect } from "react-redux";

import * as options from "../../actions/actions/personActions";

import Title from "../../UI/Title";
import GoBackButton from "../../UI/GoBackButton";

class Create extends Component {
  state = {
    firstName: "",
    lastName: "",
    age: "",
    email: "",
    phoneNumber: "",
    city: "",
    postalCode: "",
    createSuccess: false,
    showError: false,
    createMessage: ""
  };

  /**
   * Capitalizes the first character in the string.
   * @param string The string to capitalize
   */
  capitalizeFirstLetter = string => {
    return string.charAt(0).toUpperCase() + string.slice(1);
  };

  /**
   * Used for validating the created person.
   * @param {person} person The created person to validate
   */
  handleValidation = person => {
    let validation = "";

    //#region FirstName

    // Validates FirstName after the following pattern: "Nils", "Nils-Erik".
    validation = person.FirstName.match(/^([\w\-\w]){2,19}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        showError: true,
        createMessage: `${person.FirstName} is invalid:\n
            Can only contain letters and one '-' (optional)\n
            Between 2 to 20 characters long\n
            Examples: "Nils", "Nils-Erik"`
      });
    }

    // This is to guarantee the name-standard will be held the same. (Ex: "Nils", "Nils-Erik")
    person.FirstName = person.FirstName.toLowerCase();

    if (person.FirstName.includes("-")) {
      const firstNameArray = person.FirstName.split("-");
      let firstArray = firstNameArray[0];
      let secondArray = firstNameArray[1];

      firstArray = this.capitalizeFirstLetter(firstArray);
      secondArray = this.capitalizeFirstLetter(secondArray);

      person.FirstName =
        this.capitalizeFirstLetter(firstArray) +
        "-" +
        this.capitalizeFirstLetter(secondArray);
      //   person.FirstName = firstArray + "-" + secondArray;
    } else {
      person.FirstName = this.capitalizeFirstLetter(person.FirstName);
    }

    //#endregion

    //#region LastName

    // Validates LastName after the following pattern: "Jönsson", "Samuelsson".
    validation = person.LastName.match(/^([A-Öa-ö]){2,29}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        showError: true,
        createMessage: `${person.LastName} is invalid:\n
        Can only contain letters\n
        Between 2 to 30 characters long\n
        Examples: "Jönsson", "Samuelsson"`
      });
    }

    person.LastName = person.LastName.toLowerCase();
    person.LastName = this.capitalizeFirstLetter(person.LastName);

    //#endregion

    //#region Age

    // Convert it to make sure it's a number.
    let age = Number(person.Age);

    if (age > 110 || age < 5) {
      return `${age} is an invalid age: \nAge must be between 5 to 110 years old.`;
    }

    person.Age = age;

    //#endregion

    // Not validating Email since that will be done on back-end.

    //#region PhoneNumber

    person.PhoneNumber = person.PhoneNumber.replace(/[\s-.,]/g, "");

    validation = person.PhoneNumber.match(/^[\d]{4,12}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        showError: true,
        createMessage: `${person.PhoneNumber} is invalid:\n
        Can only contain numbers\n
        Must bestow of between 4 to 12 characters.`
      });
    }

    //#endregion

    //#region City

    validation = person.City.match(/^[A-Öa-ö ]{2,60}/g);

    if (validation === null || validation === undefined) {
      this.setState({
        showError: true,
        createMessage: `${person.City} is invalid:\nFormat must be as follows:\n
        Exist of only letters\n
        With a length between 2 to 60 characters\n
        Examples: "Test", "Test City"`
      });
    }

    person.City = person.City.toUpperCase();

    //#endregion

    //#region PostalCode

    validation = person.PostalCode.match(/^([\d]{1,5}[-]{0,1}[\d]{0,5})$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        showError: true,
        createMessage: `${person.PostalCode} is invalid:\n
        Format must be as follows:\n
        Only numbers and -\n
        Examples: XXXXX & XXXXX-XXXXX`
      });
    }

    //#endregion

    return person;
  };

  handleChange = event => {
    // ToDo
    const { name, value } = event.target;

    this.setState({ [name]: value, createMessage: "", showError: false });
  };

  handleSubmit = event => {
    // ToDo
    event.preventDefault();

    let validation = null;

    const {
      firstName,
      lastName,
      age,
      email,
      phoneNumber,
      city,
      postalCode,
      showError
    } = this.state;

    const person = {
      FirstName: firstName,
      LastName: lastName,
      Age: age,
      Email: email,
      PhoneNumber: phoneNumber,
      City: city,
      PostalCode: postalCode
    };

    console.log("person", person);

    validation = this.handleValidation(person);

    if (!showError) {
      this.props.onCreateSubmit(validation);

      this.setState({
        createSuccess: true,
        createMessage: "Person was successfully created."
      });
    }
  };

  render() {
    const {
      firstName,
      lastName,
      age,
      email,
      phoneNumber,
      city,
      postalCode,
      createMessage,
      createSuccess,
      showError
    } = this.state;

    return (
      <React.Fragment>
        <Title Title="Creating new person" />
        <GoBackButton />

        {showError ? (
          createMessage.includes("\n") ? (
            createMessage
              .split("\n")
              .map(msg => (
                <p className="text-danger font-weight-bold container">{msg}</p>
              ))
          ) : (
            <p className="text-danger font-weight-bold container">
              {createMessage}
            </p>
          )
        ) : null}

        {createSuccess ? (
          <p className="text-success font-weight-bold container">
            {createMessage}
          </p>
        ) : null}

        <div className="col-3 shadow p-2">
          <h3>Create</h3>

          <form onSubmit={this.handleSubmit}>
            <div className="form-group">
              <label>Firstname</label>
              <input
                name="firstName"
                type="text"
                value={firstName}
                onChange={this.handleChange}
                className="form-control"
                required
                autoFocus
              />
            </div>
            <div className="form-group">
              <label>Lastname</label>
              <input
                name="lastName"
                type="text"
                value={lastName}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>Age</label>
              <input
                name="age"
                type="number"
                value={age}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>Email</label>
              <input
                name="email"
                type="text"
                value={email}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>Phonenumber</label>
              <input
                name="phoneNumber"
                type="text"
                value={phoneNumber}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>City</label>
              <input
                name="city"
                type="text"
                value={city}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>Postal code</label>
              <input
                name="postalCode"
                type="text"
                value={postalCode}
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>

            <input
              type="submit"
              value="Submit"
              className="btn btn-primary btn-sm"
            />
          </form>
        </div>
      </React.Fragment>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    onCreateSubmit: person => dispatch(options.CreatePersonAsync(person))
  };
};

export default connect(
  null,
  mapDispatchToProps
)(Create);
