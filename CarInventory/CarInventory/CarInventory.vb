Option Strict On

''' <summary>
''' Author: Punit Patel
''' file: Lab#4 Car Inventory
''' Description:This form application is proposed to enter the details of the car and maintain a track in one operation.
''' References: CustomerList(Provided @ DC-Connect)
''' </summary>
Public Class CarInventory


    Private carList As New SortedList
    Private currentCarIdentificationNumber As String = String.Empty
    Private editMode As Boolean = False

    ''' <summary>
    ''' btnEnter_Click - Will validate that the data entered into the controls is appropriate.
    '''                - Once the data is validated a car object will be create using the  
    '''                - parameterized constructor. It will also insert the new car object
    '''                - into the carList collection. It will also check to see if the data in
    '''                - the controls has been selected from the listview by the user. In that case
    '''                - it will need to update the data in the specific car object and the 
    '''                - listview as well.
    ''' </summary>
    ''' <param name="sender">Object</param>
    ''' <param name="e">EventArgs</param>
    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click

        Dim car As Car
        Dim carItem As ListViewItem


        If IsValidInput() = True Then


            editMode = True

            ' 
            lblResult.Text = "It worked!"

            Dim carprice As Double
            Double.TryParse(txtPrice.Text.Trim, carprice)
            carprice = Math.Round(carprice, 2)
            Dim price As String = CStr(carprice)

            If currentCarIdentificationNumber.Trim.Length = 0 Then


                car = New Car(cmbMake.Text, txtModel.Text, cmbYear.Text, txtPrice.Text, chkNew.Checked)


                carList.Add(car.IdentificationNumber.ToString(), car)

            Else

                car = CType(carList.Item(currentCarIdentificationNumber), Car)


                car.Make = cmbMake.Text
                car.Model = txtModel.Text
                car.Price = txtPrice.Text
                car.Year = cmbYear.Text
                car.newStatus = chkNew.Checked
            End If

            lvwCar.Items.Clear()


            For Each carEntry As DictionaryEntry In carList


                carItem = New ListViewItem()


                car = CType(carEntry.Value, Car)


                carItem.Checked = car.newStatus
                carItem.SubItems.Add(car.IdentificationNumber.ToString())
                carItem.SubItems.Add(car.Make)
                carItem.SubItems.Add(car.Model)
                carItem.SubItems.Add(car.Year)
                carItem.SubItems.Add("$" + car.Price)



                lvwCar.Items.Add(carItem)

            Next carEntry




            Reset()

            editMode = False

        End If

    End Sub

    ''' <summary>
    ''' Reset - set the controls back to their default state.
    ''' </summary>
    Private Sub Reset()


        txtModel.Text = String.Empty
        txtPrice.Text = String.Empty
        chkNew.Checked = False
        cmbMake.SelectedIndex = -1
        cmbYear.SelectedIndex = -1
        lblResult.Text = String.Empty

        currentCarIdentificationNumber = String.Empty

    End Sub

    ''' <summary>
    ''' IsValidInput - validates the data in each control to ensure that the user has entered apprpriate values
    ''' </summary>
    ''' <returns>Boolean</returns>
    Private Function IsValidInput() As Boolean

        Dim returnValue As Boolean = True
        Dim outputMessage As String = String.Empty

        If cmbMake.SelectedIndex = -1 Then

            outputMessage += "Please select the car's make." & vbCrLf


            returnValue = False

        End If

        If cmbYear.SelectedIndex = -1 Then

            outputMessage += "Please select the car's year." & vbCrLf


            returnValue = False

        End If


        If txtPrice.Text.Trim.Length = 0 Then


            outputMessage += "Please enter the car's Price." & vbCrLf

            returnValue = False

        End If


        If txtModel.Text.Trim.Length = 0 Then


            outputMessage += "Please enter the car's Model." & vbCrLf


            returnValue = False

        End If

        Dim price As Double

        If Not Double.TryParse(txtPrice.Text, price) Then
            outputMessage += "Please enter the car's price in numeric." & vbCrLf

            returnValue = False

        End If

        If price < 0 Then
            outputMessage += "Please enter the car's price greater than 0." & vbCrLf

            returnValue = False
        End If

        If returnValue = False Then


            lblResult.Text = "ERRORS" & vbCrLf & outputMessage

        End If



        Return returnValue

    End Function

    ''' <summary>
    ''' Event is declared as private because it is only accessible within the form
    ''' The code in the btnReset_Click EventHandler will clear the form and set
    ''' focus back to the input text box. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        Reset()

    End Sub



    ''' <summary>
    ''' lvwCar_ItemCheck - used to prevent the user from checking the check box in the list view
    '''                        - if it is not in edit mode
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lvwCar_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvwCar.ItemCheck


        If editMode = False Then


            e.NewValue = e.CurrentValue

        End If

    End Sub

    ''' <summary>
    ''' lvwCar_SelectedIndexChanged - when the user selected a row in the list it will populate the fields for editing
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lvwCar_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwCar.SelectedIndexChanged


        Const identificationSubItemIndex As Integer = 1


        currentCarIdentificationNumber = lvwCar.Items(lvwCar.FocusedItem.Index).SubItems(identificationSubItemIndex).Text

        Dim car As Car = CType(carList.Item(currentCarIdentificationNumber), Car)

        txtModel.Text = Car.Model
        txtPrice.Text = Car.Price
        cmbMake.Text = Car.Make
        cmbYear.Text = Car.Year
        chkNew.Checked = Car.newStatus

        lblResult.Text = Car.GetSalutation()


    End Sub



    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

End Class
